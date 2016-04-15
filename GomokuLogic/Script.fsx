#load "gamelogic.fs"
open GomokuLogic
open Gomoku

let game = InitGame()

let moves = List.zip [3..6] [3..6]

let plfun = List.zip (List.init 4 (fun _ -> Black)) moves

let g = List.fold (fun st t -> PlayerMove t st) game plfun

let prnt (g:Game) =
   printfn "%A" g.Gamest
   printfn "  _ _ _ _ _ _ _ _ _ _ _ _ _ _ _"
   let prntP p = 
      match p with
      | None -> printf "*"
      | Some White -> printf "W"
      | Some Black -> printf "B"
   for i in 0 ..14 do
      printf "|"
      for j in 0 .. 14 do
         printf " "
         prntP (g.Gamebd.[i,j])
      printfn " |"
   printfn "  - - - - - - - - - - - - - - -"