using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.VFX;
using System.Linq;

public class NPCManager : MonoBehaviour
{
    //spawning variables:
    public List<Transform> NPCspawnPoints = new List<Transform>();
    public List<Transform> CARspawnPoints = new List<Transform>();
    public List<Transform> NPC_prefabs = new List<Transform>();
    public List<Transform> CAR_prefabs = new List<Transform>();
    List<NPC> NPCList = new List<NPC>();
    int NPCspawnPointIndex = 0;
    int CARspawnPointIndex = 0;

    //general variables:
    public float baseSpeed = 4;
    public int nrOfActiveNPCs = 40;
    public int nrOfActiveCARs = 8;

    public class NPC
    {
        public Transform pos;
        public float speed;
    }

    private void Start()
    {
        Invoke("RandomSpawnNPC", 1f);
        Invoke("RandomSpawnCAR", 3f);
    }

    void RandomSpawnNPC ()
    {
        int prefabIndex = UnityEngine.Random.Range(0, NPC_prefabs.Count);

        Transform NPCTransform = Instantiate(
            NPC_prefabs[prefabIndex], 
            NPCspawnPoints[NPCspawnPointIndex].position + new Vector3(-20f, 1 + UnityEngine.Random.Range(-1f, 1f), -20f), 
            Quaternion.identity
        );

        //NPCTransform.localScale = new Vector3(NPCTransform.localScale.x * UnityEngine.Random.Range(0.5f, 2f), NPCTransform.localScale.y * UnityEngine.Random.Range(0.5f, 2f), NPCTransform.localScale.z * UnityEngine.Random.Range(0.5f, 2f));
        if (NPCspawnPointIndex < NPCspawnPoints.Count - 1)
            NPCspawnPointIndex++;
        else
            NPCspawnPointIndex = 0;

        NPCList.Add(new NPC
        {
            pos = NPCTransform,
            speed = UnityEngine.Random.Range(baseSpeed, baseSpeed * 2)
        });

        if (NPCList.Count <= nrOfActiveNPCs + nrOfActiveCARs) 
            Invoke("RandomSpawnNPC", UnityEngine.Random.Range(0.2f, 1f));
    }

    void RandomSpawnCAR ()
    {
        int carSpawnIndex = UnityEngine.Random.Range(0, CAR_prefabs.Count);
            
        Transform CARTransform = Instantiate(
            CAR_prefabs[carSpawnIndex],
            CARspawnPoints[0].position + new Vector3(-20f, -0.5f + UnityEngine.Random.Range(0.1f, 0.4f), -20f),
            Quaternion.identity
                );

        NPCList.Add(new NPC
        {
            pos = CARTransform,
            speed = UnityEngine.Random.Range(2 + baseSpeed, baseSpeed * 3)
        });

        //CARTransform.localScale = new Vector3(CARTransform.localScale.x * UnityEngine.Random.Range(0.5f, 10f), CARTransform.localScale.y * UnityEngine.Random.Range(0.5f, 2f), CARTransform.localScale.z * UnityEngine.Random.Range(0.5f, 2f));

        //if (CARspawnPointIndex < CARspawnPoints.Count - 1)
        //    CARspawnPointIndex++;
        //else
        //    CARspawnPointIndex = 0;

        if (NPCList.Count(car => car.pos.gameObject.tag == "CAR") <= nrOfActiveCARs)
            Invoke("RandomSpawnCAR", UnityEngine.Random.Range(3f, 15f));
    }

    private void Update()
    {
        NativeArray<float2> NPCpostitions = new NativeArray<float2>(NPCList.Count, Allocator.TempJob);
        NativeArray<float> NPCSpeed = new NativeArray<float>(NPCList.Count, Allocator.TempJob);
        NativeArray<bool> NPCFlip = new NativeArray<bool>(NPCList.Count, Allocator.TempJob);
        float deltaTime = Time.deltaTime;

        for (int i = 0; i < NPCList.Count; i++)
        {
            NPCpostitions[i] = new Vector2(NPCList[i].pos.position.x, NPCList[i].pos.position.y);
            NPCSpeed[i] = NPCList[i].speed;
            NPCFlip[i] = false;
        }

        MoveNPCs moveNPCs = new MoveNPCs
        {
            NPC_pos = NPCpostitions,
            NPC_speed = NPCSpeed,
            NPC_flip = NPCFlip,
            delta_time = Time.deltaTime
        };

        JobHandle jobHandle = moveNPCs.Schedule(NPCList.Count, 5);
        jobHandle.Complete();

        for (int i = 0; i < NPCList.Count; i++)
        {
            NPCList[i].pos.position = new Vector3(NPCpostitions[i].x, NPCpostitions[i].y, -25f);
            
            if (NPCFlip[i])
            {
                if (NPCList[i].pos.gameObject.tag == "CAR")
                {
                    NPCList[i].pos.localScale = new Vector3(-NPCList[i].pos.localScale.x, NPCList[i].pos.localScale.y, NPCList[i].pos.localScale.z);
                    if (NPCList[i].speed < 0)
                        NPCList[i].pos.position = new Vector3(NPCList[i].pos.position.x, NPCList[i].pos.position.y - 3f, NPCList[i].pos.position.z);
                    else
                        NPCList[i].pos.position = new Vector3(NPCList[i].pos.position.x, NPCList[i].pos.position.y + 3f, NPCList[i].pos.position.z);
                    NPCList[i].speed = -NPCList[i].speed;
                }
                else
                {
                    NPCList[i].pos.localScale = new Vector3(-NPCList[i].pos.localScale.x, NPCList[i].pos.localScale.y, NPCList[i].pos.localScale.z);
                    NPCList[i].speed = -NPCList[i].speed;
                }
            }
        }

        NPCpostitions.Dispose();
        NPCSpeed.Dispose();
        NPCFlip.Dispose();
    }
}

[BurstCompile]
public struct MoveNPCs : IJobParallelFor
{
    public NativeArray<float2> NPC_pos;
    public NativeArray<float> NPC_speed;
    public NativeArray<bool> NPC_flip;
    public float delta_time;

    public void Execute(int index)
    {
        NPC_pos[index] += new float2(NPC_speed[index] * delta_time, 0);

        if (NPC_pos[index].x >= 30 || NPC_pos[index].x <= -40)
            NPC_flip[index] = true;
    }
}
