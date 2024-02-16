// Api Connection 
private readonly string rawgApiKey = "755e1f2dda34491da4ac33116d2608d0";
private readonly string rawgApiURL = "https://api.rawg.io/api/games";

string apiUrl = $"{rawgApiURL}?key={rawgApiKey}&search={nomJeu}";

Console.WriteLine(apiUrl);
