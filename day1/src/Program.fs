﻿// Learn more about F# at http://fsharp.org

open System.IO
open System
open AdventOfCode2017

let inputChars (filename:string) =
    // Read in a file with StreamReader.
    seq {        
        use stream = new StreamReader(filename)
        while (stream.Peek() >= 0) do
            yield stream.Read() |> char        
    }

[<EntryPoint>]
let main argv =
    if argv.Length = 0 then
        printf "Usage: day1 <inputfile>"
        1
    else if not (File.Exists(argv.[0])) then 
        printfn "Cannot find file: '%s'" argv.[0]
        2
    else
        let ints =
            argv.[0]
            |> inputChars 
            |> Seq.map (Char.GetNumericValue >> int)
            |> Seq.cache
        ints
        |> Seq.head
        |> Seq.singleton
        |> Seq.append ints 
        |> Seq.pairwise
        |> CalcCaptcha 
        |> printfn "Solution part 1: %i"
        ints
        |> Halfwise
        |> CalcCaptcha 
        |> printfn "Solution part 2: %i"
        Console.ReadKey |> ignore
        0 // return an integer exit code
