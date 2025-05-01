// Chandler Wixom, Final Project CS1400, 4/28/2025

// to do
// Load all songs
// Load playlists
//difffernt sortings forward and backward
// album
// title
//date added
//date released
//custom order
//insert and remove
//re arange custom order
// save custom order
//what to do with duplicates?
//play songs MEDAI PLAY???
// play in order
// suffle NO REPEAT
//loop????
//Exit
//save playlists 


using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System;
using NAudio;
using NAudio.Wave;


Console.Clear();
bool loop = true;
while (loop)
{
    var songs = ReadAllSongs();
    var playlists = LoadPlaylists();

    Console.WriteLine("Songs and playlists have been loaded");
    Console.WriteLine("What would you like to do");
    Console.WriteLine("1: Play Album\n2: Shuffle Album\n3: Play Song\n4: Play Playlist\n5: Shuffle Playlist\n6: Create Playlist\n7: View Playlists\n8: Edit Playlist\n9: Quit");
    int choise = -1;
    while (!Int32.TryParse(Console.ReadLine(), out choise))
    {
        Console.WriteLine("Hmm?");
    }
    switch (choise)
    { 
        case -1 : Console.WriteLine("something broke???");
            break;

        case 1: PlayAlbum(songs, false); // searches for and plays album
            break;
        
            case 2: PlayAlbum(songs, true);; // searches for and shuffles album
            break;

            case 3:PlaySong(songs); // search for and plays a specific song
            break;

            case 4:PlayPlaylist(playlists, false) ; // search for and play playlist
            break;

            case 5:
            PlayPlaylist(playlists, true); // search for and shuffle playlist
            break;

            case 6: MakePlaylist(songs); // lets you make new playlist
            break;

            case 7: WritePlaylists(playlists, true); // view playlists
            break;

        case 8: SortPlaylist(playlists); //sort playlist
            break;

        case 9: loop = false;
            break;

            default: Console.WriteLine("Sorry Don't Know That");
            Thread.Sleep(1000); // pauses for a second before clearing so you can read error message
            break ;
    
    
    
    }
    


    
}
Console.Clear();


static void CustomOrder(List<List<string>> playlist, string playlistName)
{

    
    string input = " ";
    do
    {

        List<string> wipPlaylist = new List<string>();
        foreach (var item in playlist)
        {
            wipPlaylist.Add(item[2].Substring(3));


        }
        Console.Clear();
        Console.WriteLine();
        int count = 1;
        foreach (var item in wipPlaylist)
        {

            Console.WriteLine($"{count} {item}");
            count++;
        }


        int mover = -1;
        Console.SetCursorPosition(0, 0);
        Console.Write("What song Would you like to move? :");
        while (!Int32.TryParse(Console.ReadLine(), out mover))
        {
            Console.Write("Hmm?");
        }

        int movie = -1;
        Console.SetCursorPosition(0, 0);
        Console.Write("What song Would you like to set infront of? :");
        while (!Int32.TryParse(Console.ReadLine(), out movie))
        {
            Console.Write("Hmm?");
        }
        Console.Clear();
        movie = Math.Clamp(movie - 2, 0, playlist.Count - 1);
        mover = Math.Clamp(mover - 1, 0, playlist.Count - 1);
        var temp = playlist[mover];
        playlist.RemoveAt(mover);
        playlist.Insert(movie, temp);
        Console.Clear();

        Console.Write("Song Moved - Enter to continue - Q to quit - S to save edits");

        wipPlaylist.Clear();
        foreach (var item in playlist)
        {
            wipPlaylist.Add(item[2].Substring(3));


        }

        Console.WriteLine();
        count = 1;
        foreach (var item in wipPlaylist)
        {

            Console.WriteLine($"{count} {item}");
            count++;
        }
        input = Console.ReadLine().ToLower();


    } while (!(input == "q" || input == "s"));

    if (input == "s")
    {
        List <string> list = new List<string>();

        foreach (var item in playlist)
        {
            list.Add(string.Join("¦", item));
        }
        
        File.WriteAllLines($"music\\playlists\\{playlistName}", list.ToArray());
        Console.WriteLine($"Playlist {playlistName} saved");
        Thread.Sleep(1000);
        Console.Clear();

    }
    else
    {
        Console.Clear();
    }
}

// allowed you to manipulate playlist

static void SortPlaylist(Dictionary<string, List<List<string>>> bigList)
{
    Console.Clear();
    WritePlaylists(bigList, false);
    string playlist = PickPlaylist(bigList);
    Console.Clear();

    Console.WriteLine($"How would you like to edit {playlist}");
    Console.WriteLine($"1: Reorder Playlist");

    int choise = -1;
    while (!Int32.TryParse(Console.ReadLine(), out choise))
    {
        Console.WriteLine("Hmm?");
    }

    List<List<string>> editPlaylist = new List<List<string>>(bigList[playlist]);



    switch (choise)
    {
        case -1:
            Console.WriteLine("something broke???");
            break;

        case 1:
            CustomOrder(editPlaylist, playlist);
            ; // custom

            break;

        case 2:
            ; // album
            //MISSSING
            break;
       

    }
    Console.Clear();
}



// Pick playlist
static string PickPlaylist(Dictionary<string, List<List<string>>> bigList)
{
    Console.Clear();
    string[] albums = WritePlaylists(bigList, false);
    Console.WriteLine("\nWhat playlist would you like to pick?");
    int choise = -1;
    while (!Int32.TryParse(Console.ReadLine(), out choise))
    {
        Console.WriteLine("Hmm?");
    }
    string output = albums[choise - 1];
    return output;


}



// writes the songs from a playlist 

static List<(string, string)> WriteSongsPlaylist(Dictionary<string, List<List<string>>> bigList, string playlist)
{
    List <List <string>> songs = new List <List <string>>(bigList[playlist]);

    List <(string,string)> songNames = new List <(string, string)>();
    foreach (var song in songs)
    {
        (string, string) temp = (song[2], song[1]) ;
        songNames.Add(temp);
    }

    return songNames;
}

// play playlist
static void PlayPlaylist(Dictionary<string, List<List<string>>> bigList, bool shuffle)
{
   string playlist = PickPlaylist(bigList);
    List <(string, string)> songs = WriteSongsPlaylist(bigList, playlist);
    Console.Clear();
    Console.WriteLine($"--------- {playlist} ---------\n");
    if (shuffle)
    {
        Random random = new Random();
        int albumLength = songs.Count;
        List<int> tempInts = new List<int>();

        for (int i = 0; i < albumLength;)
        {
            int temp = random.Next(0, albumLength);
            if (!tempInts.Contains(temp))
            {
                tempInts.Add(temp);
                i++;
            }

        }
        List<(string,string)> shuffled = new List<(string,string)>();
        foreach (int i in tempInts)
        {
            shuffled.Add(songs[i]);
        }
        ;
        songs = shuffled;
    }
    string feadback = "---\t\t Enter to skip\tQ to exit";

    if (shuffle) { feadback = "--- Shuffled\t\t Enter to skip\tQ to exit"; }

    Console.WriteLine(feadback);
    foreach (var song in songs)
    {
        Console.WriteLine(song);
    }
    Console.SetCursorPosition(8, 1);


    string userout = "-1";
    int songPos = 0;
    do
    {

        Console.WriteLine($"Now Playing: {songs[songPos]}");

        using (var audiofile = new AudioFileReader($"music\\{songs[songPos].Item2}\\{songs[songPos].Item1}"))
        {
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audiofile);
                outputDevice.Play();

                userout = Console.ReadKey(true).KeyChar.ToString();
                outputDevice.Stop();
            }

        }


        songPos++;
        Console.SetCursorPosition(8, 1);
        Console.WriteLine("                                                                   ");
        Console.SetCursorPosition(8, 1);


    } while (songPos < songs.Count && userout != "q");

    Console.Clear();
    Console.WriteLine("Finished Playing\n");





}


//writes the playlists available
static string[] WritePlaylists(Dictionary<string, List<List<string>>> playlists, bool wait)
{
    List<string> albums = new List<string>(playlists.Keys);

    int number = 1;
    Console.Clear();
    foreach (string album in playlists.Keys)
    {
        Console.WriteLine($"{number}: {album}");
        number++;
    }

    if (wait)
    { 
    Console.ReadKey(true);
    Console.Clear();
    }
    return albums.ToArray();
}




// loads the playlists into a dictionary (key is playlist name) Dictionary<string, List<string>
// key is playlist, list is the songs and the songs information
static Dictionary<string, List<List<string>>> LoadPlaylists()
{
   List<string> playlistsPaths = Directory.GetFiles("music\\playlists").ToList<string>();
    Dictionary < string, List<List <string>>> albums = new Dictionary< string, List< List <string>> >();

    foreach (string playlist in playlistsPaths)
    {
        string playlistName = playlist.Split('\\')[2];
        List<string> info = File.ReadAllLines(playlist).ToList();

        List<List<string>> deepInfo = new List<List<string>>();

        foreach (string infoPath in info)
        {
            List <string> tempvalues = new List<string>();
            tempvalues.AddRange(infoPath.Split('¦').ToList());
            deepInfo.Add(tempvalues);
        }
        albums.Add(playlistName, deepInfo);

    }
    return albums;
    
}

//makes a new playlist and saves when done
static void MakePlaylist(Dictionary<string, (string, string[])> bigList)
{
    Console.Clear();

    List <string> playlist = new List<string>();
    int songNumber = 1;

    string userImput = null;
    do
    {
        List<string> song = new List<string>();
        song.Add(songNumber.ToString());
        song.Add( String.Join("¦", PickSong(bigList)));
        string time = DateTime.Now.ToString("g");
        song.Add(time);
       
        playlist.Add(String.Join("¦", song));
        songNumber++;

        Console.Clear();
        Console.WriteLine($"{song[1]} added to playlist\nq to leave\ts to save\tEnter to continue");
        userImput = Console.ReadLine().ToLower();
        Console.Clear();

    } while (userImput  != "q" && userImput != "s" );
    
    if (userImput == "s")
    {
        Console.WriteLine("What would you like to name this Playlist: ");

        string playlistName = Console.ReadLine();
        File.WriteAllLines($"music\\playlists\\{playlistName}.txt", playlist.ToArray());
        Console.WriteLine($"Playlist {playlistName} saved");
        Thread.Sleep(1000);
        Console.Clear() ;
        
    }
    else
    {
        Console.WriteLine("Quit");
        Thread.Sleep(1000);
        Console.Clear();
    }

}


// askes user to pick an album and returns a list of the songs in that album
static string PickAlbum(Dictionary<string, (string, string[])> bigList)
{
    Console.Clear();
    string[] albums = WriteAlbums(bigList);
    Console.WriteLine("\nWhat album would you like to pick?");
    int choise = -1;
    while (!Int32.TryParse(Console.ReadLine(), out choise))
    {
        Console.WriteLine("Hmm?");
    }
   string output = albums[choise-1];
    return output ;


}



static void PlaySong(Dictionary<string, (string, string[])> bigList)
{
    List <string> songAlbum = new List <string>(PickSong(bigList));
    Console.Clear();
    Console.WriteLine($"--------- {songAlbum[0]} ---------\n");
    Console.WriteLine($"Now Playing: {songAlbum[1]}");
   

    using (var audiofile = new AudioFileReader($"music\\{songAlbum[0]}\\{songAlbum[1]}"))
    {
        using (var outputDevice = new WaveOutEvent())
        {
            outputDevice.Init(audiofile);
            outputDevice.Play();

            Console.ReadKey(true);
            outputDevice.Stop();
        }

    }

    Console.Clear();
    Console.WriteLine("Finished Playing\n");

}
static List<string> PickSong(Dictionary<string, (string, string[])> bigList)
{
    string albume = PickAlbum(bigList);
    Console.Clear();
    var songs = WriteSongs(albume, bigList, true);
    List<string> albumSong = new List<string>();
    albumSong.Add(albume);
    Console.WriteLine("\nWhat song would you like to pick?");
    int choise = -1;
    while (!Int32.TryParse(Console.ReadLine(), out choise))
    {
        Console.WriteLine("Hmm?");
    }
    string output = songs[choise - 1];

    albumSong.Add(output);
  

    return albumSong ;

}

//"plays" an album the user picks
// suffles it if shuffle is true
static void PlayAlbum(Dictionary<string, (string, string[])> bigList, bool shuffle)
{
    string album = PickAlbum(bigList);
    var songs = WriteSongs(album,bigList, false);
    Console.Clear();
    Console.WriteLine($"--------- {album} ---------\n");
    if (shuffle)
    {
        Random random = new Random();
       int albumLength = songs.Count;
       List <int> tempInts = new List<int>();

        for (int i = 0; i < albumLength;)
        {
            int temp = random.Next(0, albumLength);
            if (!tempInts.Contains(temp))
            {
                tempInts.Add(temp);
                i ++;
            }
            
        }
        List <string> shuffled = new List<string>();
        foreach (int i in tempInts)
        {
            shuffled.Add(songs[i]);
                };
        songs = shuffled;
            }

    string feadback = "---\t\t Enter to skip\tQ to exit";

    if (shuffle) { feadback = "--- Shuffled\t\t Enter to skip\tQ to exit"; }

    Console.WriteLine(feadback);
    foreach (var song in songs)
    {
        Console.WriteLine(song);
    }
    Console.SetCursorPosition(8, 1);

    string userout = "-1";
    int songPos = 0;
    do
    {

        Console.WriteLine($"Now Playing: {songs[songPos]}");

        using (var audiofile = new AudioFileReader($"music\\{album}\\{songs[songPos]}"))
        {
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audiofile);
                outputDevice.Play();

                userout = Console.ReadKey(true).KeyChar.ToString();
                outputDevice.Stop();
            }

        }
        

        songPos++;
        Console.SetCursorPosition(8, 1);
        Console.WriteLine("                                                                   ");
        Console.SetCursorPosition(8, 1);


    } while (songPos < songs.Count && userout != "q");

    Console.Clear();
    Console.WriteLine("Finished Playing\n");





}


//PriorityQueue <string,int> playlist = new PriorityQueue<string,int>();





// Loads all of the songs in the music folder from their albums
static Dictionary<string, (string, string[])> ReadAllSongs()
{
    string[] albumsPath = Directory.GetDirectories("music");
    Dictionary<string, (string, string[])> albumList = new Dictionary<string, (string, string[])>();


    foreach (string album in albumsPath)
    {

        string[] songs = Directory.GetFiles(album);
        albumList.Add(album.Split('\\')[1], (album, songs));
    }
    return albumList;
}


//writes all of the songs in all of the albums
static void WriteAll(Dictionary<string, (string, string[])> albumList)
{
    string[] albums = albumList.Keys.ToArray();
    foreach (string songpath in albums)
    {
        
        foreach (string song in albumList[songpath].Item2)
        {
            Console.WriteLine(song.Split('\\')[2]);
        }
        Console.WriteLine();

    }
}


// writes all the album titles in the list
static string[] WriteAlbums(Dictionary<string, (string, string[])> albumList)
{
    int count = 1;
    List <string> tempalbums = new List <string>(albumList.Keys);
    tempalbums.Remove("playlists");

    string[] albums = tempalbums.ToArray();
    foreach (string album in albums)
    {
        Console.WriteLine($"{count}. {album}");
        count++;
    }
    return albums;
}

// writes the songs in an album from the album name
static List <string> WriteSongs(string name, Dictionary<string, (string, string[])> albumList, bool write)
{
    int count = 1;
    List <string> strings = new List <string>();
    foreach (string song in albumList[name].Item2)
    {
        if (write)
        {
            Console.WriteLine($"{count}. {song.Split('\\')[2]}");
        }
        strings.Add(song.Split('\\')[2]);
        count++;
    }
    return strings;
}




