module Tests

open System
open Xunit
open AdventOfCode2017

[<Theory>]
[<InlineData("1122",3)>]
[<InlineData("1111",4)>]
[<InlineData("1234",0)>]
[<InlineData("91212129",9)>]
let ``Solution 1 test`` (input:seq<char>, output:int) =
    let actual =
        let ints = input |> Seq.map (Char.GetNumericValue >> int) 
        Seq.head ints |> Seq.singleton |> Seq.append ints         
        |> Seq.pairwise
        |> CalcCaptcha 
    Assert.Equal(output,actual)
    actual

let halfwiseHelper input =
    input
    |> List.toSeq
    |> Halfwise
    |> Seq.toList

[<Fact>]
let ``Halfwise a`` () =
    Assert.Equal<list<int*int>>([(1,1);(2,2);(1,1);(2,2)], halfwiseHelper [1;2;1;2;])

[<Fact>]
let ``Halfwise b`` () =
    Assert.Equal<list<int*int>>([(1,2);(2,1);(2,1);(1,2)], halfwiseHelper [1;2;2;1;])

[<Fact>]
let ``CalcCaptcha 2 a`` () =
    Assert.Equal(6, [(1,1);(2,2);(1,1);(2,2)] |> List.toSeq |> CalcCaptcha)

[<Theory>]
[<InlineData("1212",6)>]
[<InlineData("1221",0)>]
[<InlineData("123425",4)>]
[<InlineData("123123",12)>]
[<InlineData("12131415",4)>]
let ``Solution 2 test`` (input:seq<char>, output:int) =
    let actual =
        input 
        |> Seq.map (Char.GetNumericValue >> int) 
        |> Halfwise
        |> CalcCaptcha 
    Assert.Equal(output,actual)
    actual
