using Newtonsoft.Json;
using System.Text.Json.Serialization;

public class JokeAPIModel
{
    [JsonProperty("error")]
    public bool Error { get; set; }

    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("joke")]
    public string Joke { get; set; }

    [JsonProperty("setup")]
    public string Setup { get; set; }

    [JsonProperty("delivery")]
    public string Delivery { get; set; }

    [JsonProperty("flags")]
    public JokeFlags Flags { get; set; }

    [JsonProperty("safe")]
    public bool Safe { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("lang")]
    public string Lang { get; set; }

    public string DisplayJoke
    {
        get
        {
            if (Type == "single")
                return Joke;
            else if (Type == "twopart")
                return $"{Setup}\n{Delivery}";
            return "获取失败";
        }
    }
}

public class JokeFlags
{
    [JsonProperty("nsfw")]
    public bool Nsfw { get; set; }

    [JsonProperty("religious")]
    public bool Religious { get; set; }

    [JsonProperty("political")]
    public bool Political { get; set; }

    [JsonProperty("racist")]
    public bool Racist { get; set; }

    [JsonProperty("sexist")]
    public bool Sexist { get; set; }

    [JsonProperty("explicit")]
    public bool Explicit { get; set; }
}