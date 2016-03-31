#r "..\\packages\\Suave.1.1.1\\lib\\net40\\Suave.dll"

open Suave

startWebServer defaultConfig (Successful.OK "Hello World!")
