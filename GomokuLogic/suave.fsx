#r "C:\\Users\\Asus\\Documents\\Visual Studio 2015\\Projects\\ConsoleApplication1\\packages\\Suave.1.1.2\\lib\\net40\\Suave.dll"

open Suave


open Suave.Operators

let app =
    choose
        [ Filters.GET >=> choose

            [ Filters.path "/" >=> (
                  "My main page"
                  |> Successful.OK)

              Files.browse "C:\\Users\\Asus\\Desktop\\F#SF Mentorship\\Gomoku\\GomokuLogic\\Web Parts\\index.html" ] ]

startWebServer defaultConfig app