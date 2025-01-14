﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Note : MonoBehaviour
{
    public float generateTime;
    protected float reachTime;
    protected Vector3 position;
    protected Vector3 moveVector;
    protected GameObject note;

    protected GameObject[] noteObj;
    //protected int colNum = 3;

    public Note(float reachTime, Vector3 position, GameObject note)
    {
        this.reachTime = reachTime;
        this.position = position;
        this.note = note;
        this.note.GetComponent<GameObject>();

        //noteObj = new GameObject[colNum];
    }

    public abstract bool NoteMove(int pos);
    public abstract void NoteGenerate(GameObject obj, int pos);
}

public class WideWaveNote : Note
{
    private const float animTime = 3.0f;
    private const float noteSpeed = 3.0f;

    public WideWaveNote(float reachTime, Vector3 position, GameObject note) : base(reachTime, position, note)
    {
        generateTime = this.reachTime - animTime;
        moveVector = note.transform.position.normalized;
    }
    
    public override bool NoteMove(int pos)
    {
        note.transform.position -= moveVector * noteSpeed;
        return false;
    }

    public override void NoteGenerate(GameObject colli, int pos)
    {
        
    }
}

public class VerticalWaveNote : Note
{
    private float vertPos = 0;
    private const float animTime = 3.0f;
    private const float noteSpeed = 0.1f;

    int colNum = 3;
    int vNum = 0;

    public VerticalWaveNote(float reachTime, Vector3 position, GameObject note) : base(reachTime, position, note)
    {
        generateTime = this.reachTime - animTime;
        moveVector = new Vector3(0,0,1);
        noteObj = new GameObject[colNum];
    }

    public override bool NoteMove(int pos)
    {
        for (int i = 0; i < colNum; i++)
        {       
            if (noteObj[i] != null)
            {            
                if (noteObj[i].transform.position.z < -2.0f)
                {
                    Destroy(noteObj[i]);
                }
                else
                {
                    noteObj[i].transform.position -= new Vector3(0, 0, 0.1f);                    
                }
            }
        }
        return true;      
    }

    public override void NoteGenerate(GameObject colli, int pos)
    {
        Vector3 p;

        if(pos == 1)
        {
            p = new Vector3(0.8f, StepDetermination.groundPosition.y, 24);
        }
        else
        {
            p = new Vector3(-0.8f, StepDetermination.groundPosition.y, 24);
        }

        if (noteObj[vNum] == null)
        {
            noteObj[vNum] = Instantiate(colli, p, Quaternion.identity);
            vNum++;
        }
        if(vNum >= colNum)
        {
            vNum = 0;
        }
    }
}

public class PunchNote : Note
{
    private const float animTime = 2.0f;

    public PunchNote(float reachTime, Vector3 position, GameObject note) : base(reachTime, position, note)
    {
        generateTime = this.reachTime - animTime;
    }

    public override bool NoteMove(int pos)
    {
        return false;
    }

    public override void NoteGenerate(GameObject colli, int pos)
    {

    }
}

public class LaserNote : Note
{
    private const float animTime = 3.0f;

    public LaserNote(float reachTime, Vector3 position, GameObject note) : base(reachTime, position, note)
    {
        generateTime = this.reachTime - animTime;
    }

    public override bool NoteMove(int pos)
    {
        return false;
    }

    public override void NoteGenerate(GameObject colli, int pos)
    {

    }
}

public class ThrowCubeNote : Note
{
    private float cubePos = 0;
    private const float animTime = 3.0f;
    private const float noteSpeed = 0.2f;
    private float[] upSpeed;
    private Vector3[] moveVec;

    int cNum = 0;
    int cubeNum = 4;
    float[] target = new float[4]; //飛んでくる場所へ補正

    public ThrowCubeNote(float reachTime, Vector3 position, GameObject note) : base(reachTime, position, note)
    {
        generateTime = this.reachTime - animTime;
        moveVector = note.transform.position.normalized;
        noteObj = new GameObject[cubeNum];
        moveVec = new Vector3[cubeNum];
        upSpeed = new float[cubeNum];
    }

    public override bool NoteMove(int pos)
    {
        for(int i = 0; i < cubeNum; i++)
        {
            if (noteObj[i] != null)
            {
                if (upSpeed[i] >= 0.0001f)
                {
                    noteObj[i].transform.position += new Vector3(0, upSpeed[i], 0);
                    upSpeed[i] *= 0.9f;
                    moveVec[i] = noteObj[i].transform.position 
                                 - new Vector3(target[i], JumpStart.groundPosition.y +0.5f, 0);
                    moveVec[i] = moveVec[i].normalized;
                }
                else
                {
                    if (noteObj[i].transform.position.z <= 0)
                    {
                        Destroy(noteObj[i]);
                    }
                    else
                    {
                        noteObj[i].transform.position -= moveVec[i] * noteSpeed;
                        noteObj[i].transform.Rotate(new Vector3(180f, 60f,180f) * Time.deltaTime);
                    }

                }
            }
        }
        return true;
    }

    public override void NoteGenerate(GameObject cube, int pos)
    {
        Vector3 p;
        

        if(pos == 0)
        {
            float x = Random.value;
            p = new Vector3(5.0f + x, JumpStart.groundPosition.y-0.2f, 22.0f);
            target[cNum] = 0.8f;
        }
        else
        {
            float x = Random.value;
            p = new Vector3(-5.0f - x, JumpStart.groundPosition.y - 0.2f, 22.0f);
            target[cNum] = -0.8f;
        }

        if (noteObj[cNum] == null)
        {
            noteObj[cNum] = Instantiate(cube, p, Quaternion.identity);
            //moveVec[cNum] = noteObj[cNum].transform.position - new Vector3(target[cNum], 0, 0);
            //moveVec[cNum] = moveVec[cNum].normalized;
       
            upSpeed[cNum] = 0.5f;

            cNum++;
        }
        if (cNum >= 4)
        {
            cNum = 0;
        }
    }
}