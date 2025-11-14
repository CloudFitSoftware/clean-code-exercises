using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// Paths to the CSV files
string[] csvPaths = new string[]
{
            "data1.csv",
            "data2.csv",
            "data3.csv",
            "data4.csv",
            "data5.csv"
};

var records = new List<Dictionary<string, string>>();
foreach (var csvPath in csvPaths)
{
    if (!File.Exists(csvPath))
    {
        Console.WriteLine($"CSV file not found at: {csvPath}");
        return;
    }

    // Read all lines from CSV
    var lines = File.ReadAllLines(csvPath);

    if (lines.Length < 2)
    {
        Console.WriteLine("CSV file must have at least a header and one data row.");
        return;
    }

    // Parse headers
    var headers = lines[0].Split(',');

    // Parse each row
    for (int i = 1; i < lines.Length; i++)
    {
        var values = lines[i].Split(',');
        var record = new Dictionary<string, string>();

        for (int j = 0; j < headers.Length; j++)
        {
            string key = headers[j].Trim();
            string value = j < values.Length ? values[j].Trim() : "";
            record[key] = value;
        }

        records.Add(record);
    }

}

int totalRows = records.Count;

// Age range counters
int age20to29 = 0, age30to39 = 0, age40to49 = 0, age50plus = 0;

foreach (var record in records)
{
    if (record.ContainsKey("Age") && int.TryParse(record["Age"], out int age))
    {
        if (age >= 20 && age <= 29) age20to29++;
        else if (age >= 30 && age <= 39) age30to39++;
        else if (age >= 40 && age <= 49) age40to49++;
        else if (age >= 50) age50plus++;
    }
}

// Calculate percentages
double pct20to29 = Math.Round((double)age20to29 / totalRows * 100, 2);
double pct30to39 = Math.Round((double)age30to39 / totalRows * 100, 2);
double pct40to49 = Math.Round((double)age40to49 / totalRows * 100, 2);
double pct50plus = Math.Round((double)age50plus / totalRows * 100, 2);

// Name summary (ascending by count)
var nameSummary = records
    .GroupBy(r => r["Name"])
    .Select(g => new
    {
        Name = g.Key,
        Count = g.Count(),
        Percentage = Math.Round((double)g.Count() / totalRows * 100, 2)
    })
    .OrderBy(x => x.Count)
    .ToList();

// City summary (descending by count)
var citySummary = records
    .GroupBy(r => r["City"])
    .Select(g => new
    {
        City = g.Key,
        Count = g.Count(),
        Percentage = Math.Round((double)g.Count() / totalRows * 100, 2)
    })
    .OrderByDescending(x => x.Count)
    .ToList();

// Build summary object for JSON
var summaryObject = new
{
    TotalRows = totalRows,
    AgeSummary = new
    {
        Range20to29 = new { Count = age20to29, Percentage = pct20to29 },
        Range30to39 = new { Count = age30to39, Percentage = pct30to39 },
        Range40to49 = new { Count = age40to49, Percentage = pct40to49 },
        Range50Plus = new { Count = age50plus, Percentage = pct50plus }
    },
    NameSummary = nameSummary,
    CitySummary = citySummary
};

// Convert summaries to JSON
var options = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(summaryObject, options);

// Print JSON to console
Console.WriteLine("\nFormatted JSON Summary Output:");
Console.WriteLine(json);
