using System.Diagnostics;
using Utils;

namespace Year2023.Day03;

//https://adventofcode.com/2023/day/3
public class Day03
{
    private const string Day = "Day03";

    //What is the sum of all of the part numbers in the engine schematic?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 10, true, 4361)]
    [InlineData($"{Day}\\Input.txt", 140, false, 553825)]
    public void Part1(string inputFile, int expectedNumberOfLines, bool isSampleFile, int expectedAnswer)
    {
        int calculatedAnswer = 0;
        Schematic schematic = inputFile.ToLines().ToSchematic();
        Assert.Equal(expectedNumberOfLines, schematic.Lines.Count);

        if (isSampleFile)
        {
            SampleFileAssertions();
        }

        foreach (var line in schematic.Lines)
        {
            List<PartNumber> parts = line.Value.GetValidPartNumbers(schematic.GetLineAbove(line.Key), schematic.GetLineBelow(line.Key));
            var sum = parts.Sum(x => x.Number);
            calculatedAnswer += sum;
        }

        Assert.Equal(expectedAnswer, calculatedAnswer);

        void SampleFileAssertions()
        {
            //Extra Asserts to check data extraction
            SchematicLine line1 = schematic.Lines[0];
            SchematicLine line2 = schematic.Lines[1];
            SchematicLine line3 = schematic.Lines[2];
            SchematicLine line4 = schematic.Lines[3];
            SchematicLine line5 = schematic.Lines[4];
            SchematicLine line6 = schematic.Lines[5];
            SchematicLine line7 = schematic.Lines[6];
            SchematicLine line8 = schematic.Lines[7];
            SchematicLine line9 = schematic.Lines[8];
            SchematicLine line10 = schematic.Lines[9];

#pragma warning disable xUnit2013
            //Check parts
            Assert.Equal(2, line1.PartNumbers.Count);
            Assert.Equal(467, line1.PartNumbers[0].Number);
            Assert.Equal(0, line1.PartNumbers[0].StartIndex);
            Assert.Equal(2, line1.PartNumbers[0].EndIndex);

            Assert.Equal(114, line1.PartNumbers[1].Number);
            Assert.Equal(5, line1.PartNumbers[1].StartIndex);
            Assert.Equal(7, line1.PartNumbers[1].EndIndex);

            Assert.Equal(0, line2.PartNumbers.Count);

            Assert.Equal(2, line3.PartNumbers.Count);
            Assert.Equal(35, line3.PartNumbers[0].Number);
            Assert.Equal(633, line3.PartNumbers[1].Number);

            Assert.Equal(0, line4.PartNumbers.Count);

            Assert.Equal(1, line5.PartNumbers.Count);
            Assert.Equal(617, line5.PartNumbers[0].Number);

            Assert.Equal(1, line6.PartNumbers.Count);
            Assert.Equal(58, line6.PartNumbers[0].Number);

            Assert.Equal(1, line7.PartNumbers.Count);
            Assert.Equal(592, line7.PartNumbers[0].Number);

            Assert.Equal(1, line8.PartNumbers.Count);
            Assert.Equal(755, line8.PartNumbers[0].Number);

            Assert.Equal(0, line9.PartNumbers.Count);

            Assert.Equal(2, line10.PartNumbers.Count);
            Assert.Equal(664, line10.PartNumbers[0].Number);
            Assert.Equal(598, line10.PartNumbers[1].Number);

            //Check Symbols
            Assert.Equal(0, line1.Symbols.Count);

            Assert.Equal(1, line2.Symbols.Count);
            Assert.Equal("*", line2.Symbols[0].StringRepresentation);
            Assert.Equal(3, line2.Symbols[0].Index);

            Assert.Equal(0, line3.Symbols.Count);

            Assert.Equal(1, line4.Symbols.Count);
            Assert.Equal("#", line4.Symbols[0].StringRepresentation);
            Assert.Equal(6, line4.Symbols[0].Index);

            Assert.Equal(1, line5.Symbols.Count);
            Assert.Equal("*", line5.Symbols[0].StringRepresentation);
            Assert.Equal(3, line5.Symbols[0].Index);

            Assert.Equal(1, line6.Symbols.Count);
            Assert.Equal("+", line6.Symbols[0].StringRepresentation);
            Assert.Equal(5, line6.Symbols[0].Index);

            Assert.Equal(0, line7.Symbols.Count);

            Assert.Equal(0, line8.Symbols.Count);

            Assert.Equal(2, line9.Symbols.Count);
            Assert.Equal("$", line9.Symbols[0].StringRepresentation);
            Assert.Equal(3, line9.Symbols[0].Index);

            Assert.Equal("*", line9.Symbols[1].StringRepresentation);
            Assert.Equal(5, line9.Symbols[1].Index);

            Assert.Equal(0, line10.Symbols.Count);
#pragma warning restore xUnit2013
        }
    }

    //What is the sum of all of the gear ratios in your engine schematic?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 467835)]
    [InlineData($"{Day}\\Input.txt", 93994191)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var calculatedAnswer = 0;
        Schematic schematic = inputFile.ToLines().ToSchematic();
        foreach (var line in schematic.Lines)
        {
            var gearRatio = line.Value.GetGearRatio(schematic.GetLineAbove(line.Key), schematic.GetLineBelow(line.Key));
            calculatedAnswer += gearRatio;
        }

        Assert.Equal(expectedAnswer, calculatedAnswer);
    }
}

public static class Day03Extensions
{
    public static Schematic ToSchematic(this string[] inputLines)
    {
        List<char> symbolsToCheckFor = ['*', '#', '$', '=', '&', '%', '/', '\\', '-', '+', '@'];
        List<char> digitsToCheckFor = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        Dictionary<int, SchematicLine> schematicLines = [];
        int lineNumber = 0;
        foreach (var inputLine in inputLines)
        {
            List<PartNumber> partNumbers = [];
            List<Symbol> symbols = [];

            PartNumber? numberInProgress = null;
            int inputLineIndex = 0;
            foreach (var charInLine in inputLine)
            {
                //Empty Char Scenario
                if (charInLine == '.')
                {
                    if (numberInProgress != null)
                    {
                        //End the number and add to result
                        partNumbers.Add(numberInProgress.Clone());
                        numberInProgress = null;
                    }

                    inputLineIndex++;
                    continue;
                }

                //Digit Char Scenario
                if (digitsToCheckFor.Contains(charInLine))
                {
                    if (numberInProgress == null)
                    {
                        numberInProgress = PartNumber.StartNewNumber(inputLineIndex, charInLine);
                    }
                    else
                    {
                        numberInProgress.NumberAsString += charInLine;
                        numberInProgress.EndIndex = inputLineIndex;
                    }

                    inputLineIndex++;
                    continue;
                }

                //Symbol Char scenario
                if (symbolsToCheckFor.Contains(charInLine))
                {
                    if (numberInProgress != null)
                    {
                        //End the number and add to result
                        partNumbers.Add(numberInProgress.Clone());
                        numberInProgress = null;
                    }

                    symbols.Add(new Symbol(charInLine.ToString(), inputLineIndex));
                    inputLineIndex++;
                    continue;
                }

                throw new InvalidOperationException($"Unexpected Char '{charInLine}' found");
            }

            if (numberInProgress != null)
            {
                //Line ends in a number so we need to add the last number in progress
                partNumbers.Add(numberInProgress);
            }

            schematicLines.Add(lineNumber, new SchematicLine(inputLine, partNumbers, symbols));
            lineNumber++;
        }

        return new Schematic(schematicLines);
    }

    private static PartNumber Clone(this PartNumber original)
    {
        return new PartNumber { NumberAsString = original.NumberAsString, StartIndex = original.StartIndex, EndIndex = original.EndIndex };
    }
}

[DebuggerDisplay("{Raw}")]
public record SchematicLine(string Raw, List<PartNumber> PartNumbers, List<Symbol> Symbols)
{
    public List<PartNumber> GetValidPartNumbers(SchematicLine? lineAbove, SchematicLine? lineBelow)
    {
        var symbolsToCheck = Symbols.ToList();
        symbolsToCheck.AddRange(lineAbove?.Symbols ?? []);
        symbolsToCheck.AddRange(lineBelow?.Symbols ?? []);

        List<PartNumber> result = [];
        foreach (PartNumber partNumber in PartNumbers)
        {
            if (partNumber.HasAdjacentSymbol(symbolsToCheck.ToArray()))
            {
                result.Add(partNumber);
            }
        }

        return result;
    }

    public int GetGearRatio(SchematicLine? lineAbove, SchematicLine? lineBelow)
    {
        int ratio = 0;
        var partToCheck = PartNumbers.ToList();
        partToCheck.AddRange(lineAbove?.PartNumbers ?? []);
        partToCheck.AddRange(lineBelow?.PartNumbers ?? []);
        var symbolsToCheck = Symbols.Where(x => x.StringRepresentation == "*");
        foreach (Symbol gearSymbol in symbolsToCheck)
        {
            var partsAdjecentToGear = partToCheck.Where(x => x.HasAdjacentSymbol(gearSymbol)).ToList();
            if (partsAdjecentToGear.Count == 2)
            {
                ratio += partsAdjecentToGear.First().Number * partsAdjecentToGear.Last().Number;
            }
        }

        return ratio;
    }
}

public record Schematic(Dictionary<int, SchematicLine> Lines)
{
    public SchematicLine? GetLineAbove(int lineNumber)
    {
        return lineNumber == 0 ? null : Lines[lineNumber - 1];
    }

    public SchematicLine? GetLineBelow(int lineNumber)
    {
        return lineNumber == Lines.Last().Key ? null : Lines[lineNumber + 1];
    }
}

public record Symbol(string StringRepresentation, int Index);

[DebuggerDisplay("{Number}")]
public class PartNumber
{
    public string NumberAsString { get; set; } = string.Empty;
    public int Number => string.IsNullOrWhiteSpace(NumberAsString) ? 0 : Convert.ToInt32(NumberAsString);
    public int StartIndex { get; init; }
    public int EndIndex { get; set; }

    public bool HasAdjacentSymbol(params Symbol[] symbolsToCheck)
    {
        foreach (Symbol symbol in symbolsToCheck)
        {
            if (symbol.Index >= StartIndex - 1 && symbol.Index <= EndIndex + 1)
            {
                return true;
            }
        }

        return false;
    }

    public static PartNumber StartNewNumber(int startIndex, char @char)
    {
        return new PartNumber
        {
            StartIndex = startIndex,
            EndIndex = startIndex,
            NumberAsString = @char.ToString()
        };
    }
}