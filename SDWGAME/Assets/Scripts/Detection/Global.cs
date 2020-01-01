using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *  added by forelink
 */
public static class Global
{
    public static string userId;
    public static string classId;  // math2p or math2 or ...
    public static int    nRound;
    public static string type;     // "study" or "quiz"
    public static string mediaUrl; // url with "?" -> "!", "&" -> "|" replacement
    public static string gotoUrl;  // url with "?" -> "!", "&" -> "|" replacement
}
