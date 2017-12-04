module Tests

open System
open Xunit
open AdventOfCode2017

[<Theory>]
[<InlineData("1122",3)>]
[<InlineData("1111",4)>]
[<InlineData("1234",0)>]
[<InlineData("91212129",9)>]
let ``My test`` (input:seq<char>, output:int) =
    let actual =
        let ints = input |> Seq.map (Char.GetNumericValue >> int) 
        Seq.head ints |> Seq.singleton |> Seq.append ints         
        |> CalcCaptcha
    Assert.Equal(output,actual)
    actual
