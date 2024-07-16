// MACROS
#define LOGGER                                      // Enables log print-outs

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Master class for metric logging
/// </summary>
[System.Serializable]
public class MasterLog
{
    // Timestamp
    public string timestamp;

    // Puzzle time metrics
    public float puzzle_start_time;
    public float puzzle_time;

    // Book puzzle metrics
    public int books_placed;

    // Melody puzzle metrics
    public int notes_hit;

    public MasterLog()
    {
        timestamp = DateTime.Now.ToString("dd_MM_HH_mm_s");
        //timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        books_placed = 0;
        notes_hit = 0;
    }

    public void SetupTime()
    {
        puzzle_start_time = Time.time;
    }

    public void EndTime()
    {
        puzzle_time = Time.time - puzzle_start_time;
    }

    public void UpdateBooks()
    {
        books_placed++;
    }

    public void IncrementNotes()
    {
        notes_hit++;
    }

    /// <summary>
    /// Write log to JSON file
    /// </summary>
    public void WriteLog(string sceneName)
    {
#if (LOGGER)
        Debug.Log("Creating Log...");
#endif

        // Set puzzle duration
        EndTime();

        // Check for folder
        if (!Directory.Exists("PlayerLogs"))
        {
#if (LOGGER)
            Debug.Log("PlayerLogs folder not found. Creating...");
#endif
            Directory.CreateDirectory("PlayerLogs");
        }

        string c_string = JsonUtility.ToJson(this, true);
        string filePath = $"PlayerLogs/{sceneName}_{timestamp}.json";

        File.WriteAllText(filePath, c_string);

#if (LOGGER)
        Debug.Log($"Successfully written Log to {filePath}!");
#endif
    }
}