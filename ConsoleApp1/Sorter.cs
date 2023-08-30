class Sorter
{
    public void Sort(string fileName, int partLinesCount)
    {
        var files = SplitFile(fileName, partLinesCount);
        SortResult(files);
    }
    private class LineState
    {
        public StreamReader Reader { get; set; }
        public Line Line { get; set; }
    }
    private void SortResult(string[] files)
    {
        var readers = files.Select(x => new StreamReader(x));
        try
        {
            var lines = readers.Select(x => new LineState
            {
                Line = new Line(x.ReadLine()),
                Reader = x
            }).ToList();
            using var writer = new StreamWriter("result.txt");
            while (lines.Count > 0)
            {
                var current = lines.OrderBy(x => x.Line).First();
                writer.WriteLine(current.Line.Build());
                if (current.Reader.EndOfStream)
                {
                    lines.Remove(current);
                    continue;
                }
                
                current.Line = new Line (current.Reader.ReadLine());
                
            }
        }
        finally
        {
            foreach (var reader in readers)
            {
                reader.Dispose();
            }
        }
    }

    private void SortParts(string[] files, int partLinesCount)
    {
        Line[] lines = new Line[partLinesCount]; 
        foreach (var file in files)
        {
            var strings = File.ReadAllLines(file);
            for(int i = 0; i< strings.Length; i++)
            {
                lines[i] = new Line(strings[i]);
            }
            Array.Sort(lines, 0, strings.Length);
            File.WriteAllLines(file, lines.Select(x => x.Build()));
        }
       
    }

    private IEnumerable<Line[]> Batch(IEnumerable<string> lines, int partLinesCount)
    {
        Line[] l = new Line[partLinesCount];
        int i = 0;
        foreach(var line in lines)
        {
            l[i] = new Line(line);
            i++;
            if(i == partLinesCount)
            {
                yield return l;
                i = 0;
            }

        }
        if(i != 0)
        {
            Array.Resize(ref l, i);
            yield return l;
        }


    }

    private string[] SplitFile(string fileName, int partLinesCount)
    {
        var list = new List<string>();
        int partNumber = 0;
        foreach (var batch in Batch(File.ReadLines(fileName),partLinesCount))
        {
            partNumber++;
            var partfileName = partNumber + ".txt";
            list.Add(partfileName);

            Array.Sort(batch, 0, batch.Length);
            File.WriteAllLines(partfileName, batch.Select(x => x.Build()));
        }
        return list.ToArray();
    }
}