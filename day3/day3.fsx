type Direction =
    | Down
    | Right
    | Up
    | Left


type Move = {
    Direction: Direction
    Length: int
    Remaining: int
}


type Location = {
    v: int
    x: int
    y: int
}

let NextDir d =
    match d with
    | Down -> Right
    | Right -> Up
    | Up -> Left
    | Left -> Down




let traverse (loc:Location, mov: Move) =
    match mov.Direction with
    | Down ->
        if mov.Remaining = 0 then
            (loc, { mov with Direction = NextDir mov.Direction; Length = mov.Length+1; Remaining=mov.Length+1 } )
        else
            ( { loc with v=loc.v+ 1; y=loc.y-1 }, { mov with Remaining= mov.Remaining - 1 } )
    | Right ->
        if mov.Remaining = 0 then
            (loc, { mov with Direction = NextDir mov.Direction; Remaining=mov.Length } )
        else
            ( { loc with v=loc.v+ 1; x=loc.x+1 }, { mov with Remaining= mov.Remaining - 1 } )
    | Up ->
        if mov.Remaining = 0 then
            (loc, { mov with Direction = NextDir mov.Direction; Remaining=mov.Length + 1; Length=mov.Length+1 } )
        else
            ( { loc with v=loc.v+ 1; y=loc.y+1 }, { mov with Remaining= mov.Remaining - 1 } )
    | Left ->
        if mov.Remaining = 0 then
            (loc, { mov with Direction = NextDir mov.Direction; Remaining=mov.Length } )
        else
            ( { loc with v=loc.v+ 1; x=loc.x-1 }, { mov with Remaining= mov.Remaining - 1 } )

let addNeighbors (l:Location, m:int[,]) =
    if (l.x = 0 && l.y = 0) then
        1
    else
        m.[l.x-1,l.y+1] + m.[l.x,l.y+1] + m.[l.x+1,l.y+1] +
        m.[l.x-1,l.y  ] +                 m.[l.x+1,l.y  ] +
        m.[l.x-1,l.y-1] + m.[l.x,l.y-1] + m.[l.x+1,l.y-1]
        

let traverse2 (loc:Location, mov: Move, mp:int[,]) =
    let newLoc = loc
    match mov.Direction with
    | Down ->
        if mov.Remaining = 0 then
            (newLoc, { mov with Direction = NextDir mov.Direction; Length = mov.Length+1; Remaining=mov.Length+1 } )
        else
            ( { newLoc with y=loc.y-1}, { mov with Remaining= mov.Remaining - 1 })
    | Right ->
        if mov.Remaining = 0 then
            (newLoc, { mov with Direction = NextDir mov.Direction; Remaining=mov.Length } )
        else
            ( { newLoc with x=loc.x+1 }, { mov with Remaining= mov.Remaining - 1 } )
    | Up ->
        if mov.Remaining = 0 then
            (newLoc, { mov with Direction = NextDir mov.Direction; Remaining=mov.Length + 1; Length=mov.Length+1 } )
        else
            ( { newLoc with y=loc.y+1 }, { mov with Remaining= mov.Remaining - 1 } )
    | Left ->
        if mov.Remaining = 0 then
            (newLoc, { mov with Direction = NextDir mov.Direction; Remaining=mov.Length } )
        else
            ( { newLoc with x=loc.x-1 }, { mov with Remaining= mov.Remaining - 1 } )
           
let targetVal = 289326
let init = ( { v=1; x=0; y=0 }, { Direction=Down; Length=0; Remaining=0 } )

let rec findLoc (loc,mov) =
   if loc.v=targetVal then
       loc,mov
   else
       if loc.v % 10000 = 0 then printfn "%i" loc.v
       traverse (loc,mov) |> findLoc

let result1 =
   findLoc init
   |> fst
   
let arr = Array2D.zeroCreateBased<int> -3 -3 7 7
Array2D.set arr 0 0 1

let rec findVal (loc,mov) =
    if loc.v>targetVal then
        loc,mov
    else
        if loc.v % 10000 = 0 then printfn "%i" loc.v
        let (l,m) = traverse2 (loc,mov, arr)
        let updatedLoc = { l with v = addNeighbors(l,arr) }
        Array2D.set arr updatedLoc.x updatedLoc.y updatedLoc.v
        findVal (updatedLoc,m)

let init2 = ( { v=1; x=0; y=0 }, { Direction=Right; Length=1; Remaining=1 } )
let result2 =
    findVal init2 |> fst

printfn "part 1: %i %i %i" result1.v result1.x result1.y

printfn "part 2: %i" result2.v
