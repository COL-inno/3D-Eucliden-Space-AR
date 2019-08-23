using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip d0, d1, d2, d3, d4, d5, d6, d10, d11, d12, d13, d15, d19, d20, d21, d23, d24
                            ,euclidean, longer, correct, tryagain, tapplace, logosound, taponme, newpoint;
    private static string d0_s, d1_s, d2_s, d3_s, d4_s, d5_s, d6_s, d10_s, d11_s, d12_s, 
                        d13_s, d15_s, d19_s, d20_s, d21_s, d23_s, d24_s, 
                        euclidean_s, longer_s, correct_s, tryagain_s, taponme_s, newpoint_s, idle_s;
    static LipSyncer lipSyncer;
    static AudioSource audioSrc; 
    void Start()
    {
        d0 = Resources.Load<AudioClip>("d0");
        d1 = Resources.Load<AudioClip>("d1");
        d2 = Resources.Load<AudioClip>("d2");
        d3 = Resources.Load<AudioClip>("d3");
        d4 = Resources.Load<AudioClip>("d4");
        d5 = Resources.Load<AudioClip>("d5");
        d6 = Resources.Load<AudioClip>("d6");
        d10 = Resources.Load<AudioClip>("d10");
        d11 = Resources.Load<AudioClip>("d11");
        d12 = Resources.Load<AudioClip>("d12");
        d13 = Resources.Load<AudioClip>("d13");
        d15 = Resources.Load<AudioClip>("d15");
        d19 = Resources.Load<AudioClip>("d19");
        d20 = Resources.Load<AudioClip>("d20");
        d21 = Resources.Load<AudioClip>("d21");
        d23 = Resources.Load<AudioClip>("d23");
        d24 = Resources.Load<AudioClip>("d24");
        euclidean = Resources.Load<AudioClip>("euclidean");
        longer = Resources.Load<AudioClip>("longer");
        correct = Resources.Load<AudioClip>("correct");
        tryagain = Resources.Load<AudioClip>("tryagain");
        tapplace = Resources.Load<AudioClip>("tapplace");
        logosound = Resources.Load<AudioClip>("logosound");
        taponme = Resources.Load<AudioClip>("taponme");
        newpoint = Resources.Load<AudioClip>("newpoint");

        d0_s = Resources.Load<TextAsset>("d0_s").text;
        d1_s = Resources.Load<TextAsset>("d1_s").text;
        d2_s = Resources.Load<TextAsset>("d2_s").text;
        d3_s = Resources.Load<TextAsset>("d3_s").text;
        d4_s = Resources.Load<TextAsset>("d4_s").text;
        d5_s = Resources.Load<TextAsset>("d5_s").text;
        d6_s = Resources.Load<TextAsset>("d6_s").text;
        d10_s = Resources.Load<TextAsset>("d10_s").text;
        d11_s = Resources.Load<TextAsset>("d11_s").text;
        d12_s = Resources.Load<TextAsset>("d12_s").text;
        d13_s = Resources.Load<TextAsset>("d13_s").text;
        d15_s = Resources.Load<TextAsset>("d15_s").text;
        d19_s = Resources.Load<TextAsset>("d19_s").text;
        d20_s = Resources.Load<TextAsset>("d20_s").text;
        d21_s = Resources.Load<TextAsset>("d21_s").text;
        d23_s = Resources.Load<TextAsset>("d23_s").text;
        d24_s = Resources.Load<TextAsset>("d24_s").text;
        taponme_s = Resources.Load<TextAsset>("taponme_s").text;
        newpoint_s = Resources.Load<TextAsset>("newpoint_s").text;
        euclidean_s = Resources.Load<TextAsset>("euclidean_s").text;
        longer_s = Resources.Load<TextAsset>("longer_s").text;
        correct_s = Resources.Load<TextAsset>("correct_s").text;
        tryagain_s = Resources.Load<TextAsset>("tryagain_s").text;
        idle_s = Resources.Load<TextAsset>("idle_s").text;

        audioSrc = GetComponent<AudioSource>();
        lipSyncer = GetComponent<LipSyncer>();
    }
    public static void Play(string clip) {
        switch (clip) {
        case "d0" :
            audioSrc.clip = d0;
            audioSrc.Play();
            lipSyncer.setSequence(d0_s);
            break;
        case "d1" :
            audioSrc.clip = d1;
            audioSrc.Play();
            lipSyncer.setSequence(d1_s);
            break;
        case "d2" :
            audioSrc.clip = d2;
            audioSrc.Play();
            lipSyncer.setSequence(d2_s);
            break;
        case "d3" :
            audioSrc.clip = d3;
            audioSrc.Play();
            lipSyncer.setSequence(d3_s);
            break;
        case "d4" :
            audioSrc.clip = d4;
            audioSrc.Play();
            lipSyncer.setSequence(d4_s);
            break;
        case "d5" :
            audioSrc.clip = d5;
            audioSrc.Play();
            lipSyncer.setSequence(d5_s);
            break;
        case "d6" :
            audioSrc.clip = d6;
            audioSrc.Play();
            lipSyncer.setSequence(d6_s);
            break;
        case "d10" :
            audioSrc.clip = d10;
            audioSrc.Play();
            lipSyncer.setSequence(d10_s);
            break;
        case "d11" :
            audioSrc.clip = d11;
            audioSrc.Play();
            lipSyncer.setSequence(d11_s);
            break;
        case "d12" :
            audioSrc.clip = d12;
            audioSrc.Play();
            lipSyncer.setSequence(d12_s);
            break;
        case "d13" :
            audioSrc.clip = d13;
            audioSrc.Play();
            lipSyncer.setSequence(d13_s);
            break;
        case "d15" :
            audioSrc.clip = d15;
            audioSrc.Play();
            lipSyncer.setSequence(d15_s);
            break;
        case "d19" :
            audioSrc.clip = d19;
            audioSrc.Play();
            lipSyncer.setSequence(d19_s);
            break;
        case "d20" :
            audioSrc.clip = d20;
            audioSrc.Play();
            lipSyncer.setSequence(d20_s);
            break;
        case "d21" :
            audioSrc.clip = d21;
            audioSrc.Play();
            lipSyncer.setSequence(d21_s);
            break;
        case "d23" :
            audioSrc.clip = d23;
            audioSrc.Play();
            lipSyncer.setSequence(d23_s);
            break;
        case "d24" :
            audioSrc.clip = d24;
            audioSrc.Play();
            lipSyncer.setSequence(d24_s);
            break;
        case "longer" :
            audioSrc.clip = longer;
            audioSrc.Play();
            lipSyncer.setSequence(longer_s);
            break;
        case "correct" :
            audioSrc.clip = correct;
            audioSrc.Play();
            lipSyncer.setSequence(correct_s);
            break;
        case "tryagain" :
            audioSrc.clip = tryagain;
            audioSrc.Play();
            lipSyncer.setSequence(tryagain_s);
            break;
        case "euclidean" :
            audioSrc.clip = euclidean;
            audioSrc.Play();
            lipSyncer.setSequence(euclidean_s);
            break;
        case "newpoint" :
            audioSrc.clip = newpoint;
            audioSrc.Play();
            lipSyncer.setSequence(newpoint_s);
            break;
        case "taponme" :
            audioSrc.clip = taponme;
            audioSrc.Play();
            lipSyncer.setSequence(taponme_s);
            break;
        case "tapplace" :
            audioSrc.clip = tapplace;
            audioSrc.Play();
            lipSyncer.setSequence(idle_s);
            break;
        case "logosound" :
            audioSrc.clip = logosound;
            audioSrc.Play();
            lipSyncer.setSequence(idle_s);
            break;
        }
    }
}
