namespace DataMatch.Models
{
    public class DiffModel
    {
        public string Left { get; set; }
        public string Right { get; set; }

        public DataMatchResponse Compare()
        {
            DataMatchResponse response = new();
            if (Left.Length != Right.Length)
            {
                response.DiffResultType = "SizeDoNotMatch";
                return response;
            }

            if (string.Equals(Left, Right, StringComparison.Ordinal))
            {
                response.DiffResultType = "Equals";
                return response;
            }

            List<Diff> diffs = new();
            for (int i = 0; i < Left.Length; i++)
            {
                if (Left[i] != Right[i])
                {
                    int length = 1;
                    while (Left[i + length] != Right[i + length] && i + length < Left.Length)
                        length++;
                    diffs.Add(new Diff { Offset = i, Length = length });
                    i += length - 1;
                }
            }

            response.DiffResultType = "ContentDoNotMatch";
            response.Diffs = diffs;
            return response;
        }
    }
}
