using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MessageSystem : MonoBehaviour
{
    public static string messageCopy = String.Empty;

    public static void setMessage(String newMessage)
    {
        messageCopy = newMessage;
    }
}