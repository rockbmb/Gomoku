namespace GomokuLogic

module Gomoku = 

   type Player = Black | White

   type GameState = Running of Player (*player whose move was the last*) | Draw | Win of Player

   type GameBoardT<'T> = 'T [,]

   type GameBoard = GameBoardT<option<Player>>

   type Game = {gamest : GameState;
                gamebd : GameBoard
                occupied : int} //this int represents the board's current number of pieces

   type initGame = unit -> Game

   type next = Player -> Player

   type validMove = Player * (int * int) -> Game -> bool

   type playerMove = Player * (int * int) -> Game -> Game

   type getMove = unit -> Player * (int * int)

   type matchCore = unit -> Game

   let isOver game =
      match game.gamest with
      | Win _ | Draw -> true
      | _ -> false

   let next plyr =
      match plyr with
      | White -> Black
      | Black -> White

   let initGame() =
     let board = Array2D.create 15 15 None
     {gamest = Running Black;
      gamebd = board
      occupied = 0}
   
   let getMove () = Black,(7,7)

   let validMove (plyr,(x,y)) g =
      let b = g.gamebd.[x,y].IsNone
      match g.gamest with
      | Running p -> p=plyr && b
      | _ -> false

   let playerMove (plyr,(x,y)) g =
      g.gamebd.[x,y] <- Some plyr
      let occ = g.occupied + 1
      let l = [-4..4]
      let pieces =
           let rec func ls (hor,vert,diag) = 
            
            match ls with
            | [] -> (hor, vert, diag)
            | h :: t -> func t <| match (x+h < 0 || x+h > 14), (y+h < 0 || y+h > 14) with
                                  | true,true -> (hor, vert, diag)
                                  | true, false -> (hor, g.gamebd.[x,y+h] :: vert, diag)
                                  | false, true -> (g.gamebd.[x+h,y] :: hor, vert, diag)
                                  | false, false -> (g.gamebd.[x+h,y] :: hor, g.gamebd.[x,y+h] :: vert, g.gamebd.[x+h,y+h] :: diag)
           let rec count adjacent n =
             match adjacent with
             | [] -> n
             | h :: t -> match h with
                         | None -> count t 0
                         | Some p -> if p = plyr then count t (n+1) else count t 0           
           let tripl = func l ([],[],[])
           (fun (x,y,z) -> count x 0 > 5 || count y 0 > 5 || count z 0 > 5) tripl
      if pieces then {g with gamest = Win plyr; occupied = occ}
      elif occ = 225 then {g with gamest = Draw; occupied = occ}
      else {g with gamest = Running <| next plyr; occupied = occ}
    
   
   let matchCore () =
     let game = initGame ()
     let rec helper g = 
         let move = getMove ()
         if (not <| isOver game && validMove move g) then helper <| playerMove move g
     helper game