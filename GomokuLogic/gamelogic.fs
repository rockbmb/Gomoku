namespace GomokuLogic

module Gomoku = 

   type Player = Black | White

   type Gamestate = Running of Player (*player whose move was the last*) | Draw | Win of Player

   type GameBoardT<'T> = 'T [,]

   type GameBoard = GameBoardT<option<Player>>

   type Game = {Gamest : Gamestate;
                Gamebd : GameBoard
                Occupied : int} //this int represents the board's current number of pieces

   type InitGame = unit -> Game

   type Next = Player -> Player

   type ValidMove = Player * (int * int) -> Game -> bool

   type PlayerMove = Player * (int * int) -> Game -> Game

   type GetMove = unit -> Player * (int * int)

   type MatchCore = unit -> Game

   let isOver game =
      match game.Gamest with
      | Win _ | Draw -> true
      | _ -> false

   let Next plyr =
      match plyr with
      | White -> Black
      | Black -> White

   let InitGame() =
     let board = Array2D.create 15 15 None
     {Gamest = Running Black;
      Gamebd = board
      Occupied = 0}
   
   let GetMove () = Black,(7,7)

   let ValidMove (plyr,(x,y)) g =
      let b = g.Gamebd.[x,y].IsNone
      match g.Gamest with
      | Running p -> p=plyr && b
      | _ -> false

   let PlayerMove (plyr,(x,y)) g =
      g.Gamebd.[x,y] <- Some plyr
      let occ = g.Occupied + 1
      let l = [-4..4]
      let pieces =
           let rec func ls (hor,vert,diag) = 
            
            match ls with
            | [] -> (hor, vert, diag)
            | h :: t -> func t <| match (x+h < 0 || x+h > 14), (y+h < 0 || y+h > 14) with
                                  | true,true -> (hor, vert, diag)
                                  | true, false -> (hor, g.Gamebd.[x,y+h] :: vert, diag)
                                  | false, true -> (g.Gamebd.[x+h,y] :: hor, vert, diag)
                                  | false, false -> (g.Gamebd.[x+h,y] :: hor, g.Gamebd.[x,y+h] :: vert, g.Gamebd.[x+h,y+h] :: diag)
           let rec count adjacent n =
             match adjacent with
             | [] -> n
             | h :: t -> match h with
                         | None -> count t 0
                         | Some p -> if p = plyr then count t (n+1) else count t 0           
           let tripl = func l ([],[],[])
           (fun (x,y,z) -> count x 0 > 5 || count y 0 > 5 || count z 0 > 5) tripl
      if pieces then {g with Gamest = Win plyr; Occupied = occ}
      elif occ = 225 then {g with Gamest = Draw; Occupied = occ}
      else {g with Gamest = Running <| Next plyr; Occupied = occ}
    
   
   let MatchCore () =
     let game = InitGame ()
     let rec helper g = 
         let move = GetMove ()
         if (not <| isOver game && ValidMove move g) then helper <| PlayerMove move g
     helper game