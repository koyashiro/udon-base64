using UnityEngine;
using UdonSharp;
using Koyashiro.UdonBase64;

public class UdonBase64Sample : UdonSharpBehaviour
{
    public void Start()
    {
        var bytes = UdonBase64.Decode("4Yd7aFbk9o7MCj3h9xiKhA==");

        Debug.Log(bytes.Length); //16
        Debug.Log($"0x{bytes[0]:x2}");  // 0xe1
        Debug.Log($"0x{bytes[1]:x2}");  // 0x87
        Debug.Log($"0x{bytes[2]:x2}");  // 0x7b
        Debug.Log($"0x{bytes[3]:x2}");  // 0x68
        Debug.Log($"0x{bytes[4]:x2}");  // 0x56
        Debug.Log($"0x{bytes[5]:x2}");  // 0xe4
        Debug.Log($"0x{bytes[6]:x2}");  // 0xf6
        Debug.Log($"0x{bytes[7]:x2}");  // 0x8e
        Debug.Log($"0x{bytes[8]:x2}");  // 0xcc
        Debug.Log($"0x{bytes[9]:x2}");  // 0x0a
        Debug.Log($"0x{bytes[10]:x2}"); // 0x3d
        Debug.Log($"0x{bytes[11]:x2}"); // 0xe1
        Debug.Log($"0x{bytes[12]:x2}"); // 0xf7
        Debug.Log($"0x{bytes[13]:x2}"); // 0x18
        Debug.Log($"0x{bytes[14]:x2}"); // 0x8a
        Debug.Log($"0x{bytes[15]:x2}"); // 0x84
    }
}
