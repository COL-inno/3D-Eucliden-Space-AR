using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class LipSyncer : MonoBehaviour
{
    [System.Serializable]
    public class Frame {
        public int frameNumber;    
        public int frameDelay;     
        public float[] Visemes = new float[15];      
        public float laughterScore; 
    }

    [System.Serializable]
    public class Sequence {    
        public List<Frame> entries = new List<Frame>();      
        public float length; 
    }

    public SkinnedMeshRenderer skinnedMeshRenderer;
    public int [] visemeToBlendTargets = new int[15];
    public float smilePercentageForIdle = 60.0f;
    private AudioSource audioSource;
    private Sequence currentSequence = null;
    private bool isMorphTargetSetToIdle = false;
   
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
  
    void Update()
    {
        if (!audioSource.isPlaying || isCurrentSeqIdle()) {
            SetIdleMorphTarget();
        } else {
            Frame currentFrame = getFrameAtTime(audioSource.time);       
            if (currentFrame == null) 
                SetIdleMorphTarget();
            else {
                SetVisemeToMorphTarget(currentFrame);
            }
        }
    }
    Frame getFrameAtTime(float time) {
        Frame frameToReturn = null;
        if (time < currentSequence.length && currentSequence.entries.Count > 0)
        {
            float percentComplete = time / currentSequence.length;
            frameToReturn = currentSequence.entries[(int)(currentSequence.entries.Count * percentComplete)];
        }
        return frameToReturn;
    }
    void SetIdleMorphTarget() {
        if (isMorphTargetSetToIdle)
            return;

        skinnedMeshRenderer.SetBlendShapeWeight(visemeToBlendTargets[0], smilePercentageForIdle);
        for (int i = 1; i < visemeToBlendTargets.Length; i++) {
            skinnedMeshRenderer.SetBlendShapeWeight(visemeToBlendTargets[i], 0);
        } 
        isMorphTargetSetToIdle = true;
    }
    void SetVisemeToMorphTarget(Frame frame)
    {
        for (int i = 0; i < visemeToBlendTargets.Length; i++)
        {
            if (visemeToBlendTargets[i] != -1)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(
                    visemeToBlendTargets[i],
                    frame.Visemes[i] * 100.0f);
            }
        }
        if (isMorphTargetSetToIdle)
            isMorphTargetSetToIdle = false;
    }
    public void setSequence (string jsonString) {
        Sequence seq = JsonUtility.FromJson<Sequence>(jsonString);
        currentSequence = seq;
    }
    bool isCurrentSeqIdle() {
        if (currentSequence == null)
            return true;
        else 
           return currentSequence.entries.Count == 1 && currentSequence.entries[0].Visemes[0] == 1;
    }
}
