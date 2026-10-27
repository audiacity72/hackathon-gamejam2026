using System.Collections.Generic;
using UnityEngine;

public class ObjectKeywords : MonoBehaviour
{
    [Header("Keyword Slots")]
    [Tooltip("Maximum number of keywords this object can hold.")]
    public int maxKeywords = 4;

    [Tooltip("The keywords currently assigned to this object.")]
    public List<string> activeKeywords = new List<string>();

    /// <summary>
    /// Attempts to add a keyword. Returns true if successful, false if it failed.
    /// </summary>
    public bool TryAddKeyword(string newKeyword)
    {
        // 1. Check if the slots are already full
        if (activeKeywords.Count >= maxKeywords)
        {
            Debug.Log($"[ObjectKeywords] Cannot add '{newKeyword}'. All {maxKeywords} slots are full!");
            return false;
        }

        // 2. Check if the keyword is actually valid in the game's database
        if (!KeywordDatabase.IsValidKeyword(newKeyword))
        {
            Debug.LogWarning($"[ObjectKeywords] '{newKeyword}' is not a valid recognized keyword!");
            return false;
        }

        // 3. Prevent duplicate keywords on the same object
        if (activeKeywords.Contains(newKeyword))
        {
            Debug.Log($"[ObjectKeywords] This object already has the keyword '{newKeyword}'.");
            return false;
        }

        // 4. Success! Add the keyword.
        activeKeywords.Add(newKeyword);
        Debug.Log($"[ObjectKeywords] Added '{newKeyword}'. Current keywords: {string.Join(", ", activeKeywords)}");
        return true;
    }

    /// <summary>
    /// Removes a keyword from the object.
    /// </summary>
    public void RemoveKeyword(string keywordToRemove)
    {
        if (activeKeywords.Contains(keywordToRemove))
        {
            activeKeywords.Remove(keywordToRemove);
            Debug.Log($"[ObjectKeywords] Removed '{keywordToRemove}'.");
        }
    }
}