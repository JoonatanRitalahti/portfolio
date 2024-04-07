/*
 * Author: Joonatan Ritalahti
 * Date: 23.01.2023
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings. Set in Inspector")]
    [Tooltip("Is the game in Debug mode?")]
    public bool DEBUG;
    [Tooltip("Is the game in Basic Game mode?")]
    public GameMode gameMode = GameMode.OrderMode;
    [Tooltip("Do we use damage from incorrect taps?")]
    public bool useDamageFromIncorrectTap = false;
    [Range(0.1f, 0.7f)]
    public float ButtonSize = 0.32f;
    [Tooltip("Max health for each player")]
    public int MaxHealthForPlayers = 300;

    [Header("Game Objects. Set in Inspector")]
    public GameObject ButtonPrefab;
    [Tooltip("Buttons, set in inspector")]
    public List<GameObject> ButtonPrefabs;
    [Tooltip("Canvas")]
    public GameObject ButtonParent;
    public GameObject DebugLine;
    public List<GameObject> SoundEffectObject;
    public List<GameObject> PopSounds;
    public GameObject ButtonWrong;

    //SoundPrefabLists
    public List<GameObject> PirateAngrySounds;
    public List<GameObject> PiratePeekSounds;
    public List<GameObject> PirateShockSounds;
    public List<GameObject> PirateSmileSounds;
    public List<GameObject> PirateAngrySounds2;
    public List<GameObject> PiratePeekSounds2;
    public List<GameObject> PirateShockSounds2;
    public List<GameObject> PirateSmileSounds2;

    public Animator p1Animator;
    public Animator p2Animator;

    [Tooltip("Min value for timer for animation changes")]
    public float minRange = 5;
    [Tooltip("Max value for timer for animation changes")]
    public float maxRange = 15;

    [Header("Do not set in Inspector")]
    public int AnimationEffectP1 = 0;
    public int AnimationEffectP2 = 0;

    public List<Vector3> SpawnValuesP1 = new List<Vector3>();
    public List<Vector3> SpawnValuesP2 = new List<Vector3>();
    private float _nearASize;

    public static GameManager instance;

    public PlayerManager playerManager = new PlayerManager();
    public Player Winner;
    public List<Wave> waves = new List<Wave>();
    public GameObject Player1SpawnPlane;
    public GameObject Player2SpawnPlane;



    private void Awake()
    {
        instance = this;
        CreateWaves();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DEBUG)
        {
            Debug.Log("Game is in Debug mode");
            DebugLine.SetActive(true);
        }
        else
        {
            Debug.Log("Game is in Basic mode");
        }
        ButtonParent.transform.localScale = new Vector3(ButtonSize, ButtonSize, ButtonSize);

        _nearASize = ButtonPrefabs[0].GetComponent<SphereCollider>().radius * 0.70f;
        Debug.Log("Near A size: " + _nearASize);
        playerManager.player1 = new Player(PlayerNumber.p1, 1, MaxHealthForPlayers);
        playerManager.player2 = new Player(PlayerNumber.p2, 1, MaxHealthForPlayers);

        StartCoroutine(SpawnClickables(playerManager.player1));
        StartCoroutine(SpawnClickables(playerManager.player2));

        StartCoroutine(PlayerFaceGestures(playerManager.player1));
        StartCoroutine(PlayerFaceGestures(playerManager.player2));
    }

    // Update is called once per frame
    void Update()
    {
        //If pressed 1, Deduct health from player 1
        if (Input.GetKey(KeyCode.Alpha1))
        {
            PointManager.instance.RemovePointsOnSelf(playerManager.player1, 10);
        }
        //If pressed 2, Deduct health from player 2
        if (Input.GetKey(KeyCode.Alpha2))
        {
            PointManager.instance.RemovePointsOnSelf(playerManager.player2, 10);
        }

    }
    private IEnumerator SpawnClickables(Player player)
    {

        if (gameMode == GameMode.OrderMode) //Normal gamemode
        {
            int waveNum = 1;
            foreach (var wave in waves)
            {

                Coroutine preloadSpawns = StartCoroutine(PreLoadSpawnPositions(player, wave));
                //Wait in the end. We need to do some calculations at the same time.

                yield return new WaitForSeconds(0.3f); //Wait for (X) seconds.
                yield return preloadSpawns; //Wait for button spawn to finish. Should be done.

                ButtonObject lastButton = null;
                player.NextNumber = wave.NumberOfButtons;
                player.CurrentWave = wave;
                int decCounter = wave.NumberOfButtons;
                float timeRealStartWave = Time.time;
                List<int> buttonCounter = new List<int>();
                //Shuffle buttonlist
                for (int i = 0; i < ButtonPrefabs.Count; i++)
                { buttonCounter.Add(i); }
                var shuffleOrder = buttonCounter.OrderBy(a => Guid.NewGuid()).ToList();

                for (int i = 0; i < wave.NumberOfButtons; i++)
                {
                    float timeS = Time.time;
                    GameObject gameObject;
                    if (i >= ButtonPrefabs.Count)
                    {
                        //Shoudlnt go here. Instantiate basic btn if calculations fail.
                        gameObject = Instantiate<GameObject>(ButtonPrefab);
                    }
                    else
                    {
                        if (ButtonPrefabs[i])
                            gameObject = Instantiate<GameObject>(ButtonPrefabs[shuffleOrder[i]]); //Instantiate a button as a gameobject
                        else
                        {
                            Debug.LogError("Buttonprefab is null and cannot be instantiated");
                            break;
                        }
                    }

                    gameObject.transform.SetParent(ButtonParent.transform, true); //Set buttons parent
                    if (player.playerNumber == PlayerNumber.p1) //Set the spawn location that has been previously calculated.
                    {
                        gameObject.transform.position = SpawnValuesP1[i];
                    }
                    else
                    {
                        gameObject.transform.position = SpawnValuesP2[i];
                        var targetRotation = Quaternion.LookRotation(-gameObject.transform.forward, Vector3.up);
                        gameObject.transform.rotation = targetRotation;
                    }
                    lastButton = gameObject.GetComponent<ButtonObject>();
                    lastButton.Activated = false; //Set button not pressable
                    GameObject soundSpawn = Instantiate(PopSounds[UnityEngine.Random.Range(0, PopSounds.Count)], gameObject.transform.position, Quaternion.identity);
                    Destroy(soundSpawn, 1f);

                    //Button coloring while not pressable
                    Color start = lastButton.gameObject.GetComponent<Renderer>().material.color;
                    lastButton.GetComponent<Renderer>().material.color = new Color(start.r, start.b, start.g, 0.3f);

                    //Init button

                    lastButton.Init(player, decCounter, wave.DamageToEnemyPerSmashedButton, wave, SoundEffectObject, ButtonWrong);

                    player.PlayersButtons.Add(gameObject);
                    float time = wave.TimeRequiredForWaveToForm / (wave.NumberOfButtons + 1) - (Time.time - timeS); //Calculate wait time.
                    yield return new WaitForSeconds(time);
                    decCounter--;
                }

                lastButton.WaitForClicker = lastButton.StartCoroutine(lastButton.WaitForClick(wave.TimeFromWaveStartToAutomaticClear));
                //Set all buttons active and change the color.
                foreach (var but in player.PlayersButtons)
                {
                    but.GetComponent<ButtonObject>().Activated = true;
                    Color start = but.gameObject.GetComponent<Renderer>().material.color;
                    but.GetComponent<Renderer>().material.color = new Color(start.r, start.b, start.g, 1f);

                }
                yield return lastButton.WaitForClicker;
                waveNum++;
            }
        }
    }
    /// <summary>
    /// Loads spawn locations before spawning buttons.
    /// </summary>
    /// <param name="p"></param>
    /// <param name="wave"></param>
    /// <returns></returns>
    IEnumerator PreLoadSpawnPositions(Player p, Wave wave)
    {
        if (p.playerNumber == PlayerNumber.p1)
        {
            SpawnValuesP1.Clear();

            for (int i = 0; i < wave.NumberOfButtons; i++)
            {
                Vector3 pos = CalculateSpawnPosition(playerManager.player1);
                SpawnValuesP1.Add(pos);
            }
        }
        else
        {
            SpawnValuesP2.Clear();
            for (int i = 0; i < wave.NumberOfButtons; i++)
            {
                Vector3 pos = CalculateSpawnPosition(playerManager.player2);
                SpawnValuesP2.Add(pos);
            }
        }
        yield return null;
    }
    /// <summary>
    /// Calculate players position to spawn. Take account area and distance to other buttons.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private Vector3 CalculateSpawnPosition(Player player)
    {
        int i = 0;
        List<Vector3> spawnValues;
        List<Vector3> VerticeList;

        if (player.playerNumber == PlayerNumber.p1)
        {
            VerticeList = new List<Vector3>(Player1SpawnPlane.GetComponent<MeshFilter>().sharedMesh.vertices);
            spawnValues = SpawnValuesP1;
        }
        else
        {
            VerticeList = new List<Vector3>(Player2SpawnPlane.GetComponent<MeshFilter>().sharedMesh.vertices);
            spawnValues = SpawnValuesP2;
        }
        Vector3 pos = calculateNew(player.playerNumber, VerticeList);
        while (i < 500)
        {
            bool isOk = true;
            foreach (var but in spawnValues)
            {
                if (but == null)
                {
                    break;
                }
                if (IsPositionOkay(pos, but))
                {
                    Debug.DrawLine(pos, but, Color.green, 5f);
                    continue;
                }
                else
                {
                    Debug.DrawLine(pos, but, Color.magenta, 8f);
                    isOk = false;
                    break;
                }
            }
            if (!isOk)
            {
                i++;
                pos = calculateNew(player.playerNumber, VerticeList);
                continue;
            }
            break;
        }

        return pos;
    }
    /// <summary>
    /// Calculate new position
    /// </summary>
    /// <param name="p"></param>
    /// <param name="VerticeList"></param>
    /// <returns></returns>
    private Vector3 calculateNew(PlayerNumber p, List<Vector3> VerticeList)
    {
        Vector3 position;
        Transform plane;
        if (p == PlayerNumber.p1)
        {
            plane = Player1SpawnPlane.transform;
        }
        else
        {
            plane = Player2SpawnPlane.transform;
        }

        Vector3 leftTop = plane.TransformPoint(VerticeList[0]);
        Vector3 rightTop = plane.TransformPoint(VerticeList[10]);
        Vector3 leftBottom = plane.TransformPoint(VerticeList[110]);
        Vector3 rightBottom = plane.TransformPoint(VerticeList[120]);
        Vector3 XAxis = rightTop - leftTop;
        Vector3 ZAxis = leftBottom - leftTop;
        Vector3 RndPointonPlane = leftTop + XAxis * UnityEngine.Random.value + ZAxis * UnityEngine.Random.value;
        position = RndPointonPlane + plane.up * 0.5f;

        return position;
    }
    private bool IsPositionOkay(Vector3 pos, Vector3 pos2)
    {
        float dist = Vector3.Distance(pos, pos2);
        if (dist < _nearASize)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator PlayerFaceGestures(Player player)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minRange, maxRange));
        int last = 0;

        if (player.playerNumber == PlayerNumber.p1)
        {
            while (true)
            {
                if (last == AnimationEffectP1 || last == 0)
                {
                    AnimationEffectP1 = UnityEngine.Random.Range(1, 5);
                }
                p1Animator.SetInteger("WhichEffect", AnimationEffectP1);
                last = AnimationEffectP1;
                PlayGruntSounds(p1Animator.transform.position, AnimationEffectP1, 0);
                AnimationEffectP1 = 0;
                yield return new WaitForSeconds(0.1f);
                p1Animator.SetInteger("WhichEffect", AnimationEffectP1);
                yield return new WaitForSeconds(UnityEngine.Random.Range(minRange, maxRange));
            }
        }
        else
        {
            while (true)
            {
                if (last == AnimationEffectP2 || last == 0)
                {
                    AnimationEffectP2 = UnityEngine.Random.Range(1, 5);
                }
                p2Animator.SetInteger("WhichEffect", AnimationEffectP2);
                last = AnimationEffectP2;

                PlayGruntSounds(p2Animator.transform.position, AnimationEffectP2, 1);
                AnimationEffectP2 = 0;
                yield return new WaitForSeconds(0.1f);
                p2Animator.SetInteger("WhichEffect", AnimationEffectP2);
                yield return new WaitForSeconds(UnityEngine.Random.Range(minRange, maxRange));
            }
        }
    }

    void PlayGruntSounds(Vector3 pos, int val, int pirate)
    {
        GameObject a;
        if (pirate == 0)
        {

            switch (val)
            {
                case 1:
                    {
                        a = Instantiate(PirateAngrySounds[UnityEngine.Random.Range(0, PirateAngrySounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
                case 2:
                    {
                        a = Instantiate(PiratePeekSounds[UnityEngine.Random.Range(0, PiratePeekSounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
                case 3:
                    {
                        a = Instantiate(PirateShockSounds[UnityEngine.Random.Range(0, PirateShockSounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
                case 4:
                    {
                        a = Instantiate(PirateAngrySounds[UnityEngine.Random.Range(0, PirateSmileSounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
            }
        }
        else
        {

            switch (val)
            {
                case 1:
                    {
                        a = Instantiate(PirateAngrySounds2[UnityEngine.Random.Range(0, PirateAngrySounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
                case 2:
                    {
                        a = Instantiate(PiratePeekSounds2[UnityEngine.Random.Range(0, PiratePeekSounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
                case 3:
                    {
                        a = Instantiate(PirateShockSounds2[UnityEngine.Random.Range(0, PirateShockSounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
                case 4:
                    {
                        a = Instantiate(PirateAngrySounds2[UnityEngine.Random.Range(0, PirateSmileSounds.Count)], pos, Quaternion.identity);
                        Destroy(a, 2f);
                        return;
                    }
            }
        }
    }

    private void OnGUI()
    {
        if (DEBUG)
        {
            GUI.Label(new Rect(0, 0, 100, 100), "Player 1 health: " + playerManager.player1.Health);
            GUI.Label(new Rect(0, 20, 100, 100), "Player 2 health: " + playerManager.player2.Health);
        }
    }
    /// <summary>
    /// Create Waves.
    /// Source gpt-3.
    /// </summary>
    private void CreateWaves()
    {
        Wave wave1 = new Wave(1, 2, 1, 5, 5, 4, 30, 10);
        Wave wave2 = new Wave(2, 2, 0.8f, 5, 5, 6, 30, 10);
        Wave wave3 = new Wave(3, 3, 1, 5, 5, 12, 30, 10);
        Wave wave4 = new Wave(4, 3, 0.8f, 5, 5, 12, 30, 10);
        Wave wave5 = new Wave(5, 3, 1.2f, 5, 5, 15, 30, 10);
        Wave wave6 = new Wave(6, 4, 0.8f, 5, 7, 20, 30, 10);
        Wave wave7 = new Wave(7, 4, 1.2f, 5, 7, 20, 30, 10);
        Wave wave8 = new Wave(8, 4, 1, 5, 7, 24, 30, 10);
        Wave wave9 = new Wave(9, 4, 0.8f, 5, 7, 24, 30, 10);
        Wave wave10 = new Wave(10, 4, 1, 5, 7, 24, 30, 10);
        Wave wave11 = new Wave(11, 4, 1.2f, 5, 7, 30, 30, 10);
        Wave wave12 = new Wave(12, 5, 1.2f, 5, 10, 30, 30, 10);
        Wave wave13 = new Wave(13, 5, 1.2f, 5, 10, 30, 30, 10);
        Wave wave14 = new Wave(14, 5, 1, 5, 10, 42, 30, 10);
        Wave wave15 = new Wave(15, 5, 1, 5, 10, 42, 30, 10);
        Wave wave16 = new Wave(16, 5, 0.8f, 5, 10, 42, 30, 10);
        Wave wave17 = new Wave(17, 5, 1.2f, 5, 10, 56, 30, 10);
        Wave wave18 = new Wave(18, 5, 0.8f, 5, 10, 56, 30, 10);
        Wave wave19 = new Wave(19, 5, 1, 5, 10, 6, 30, 10);
        Wave wave20 = new Wave(20, 5, 0.8f, 5, 10, 56, 30, 10);

        Wave[] allWaves = { wave1, wave2, wave3, wave4, wave5, wave6, wave7, wave8, wave9, wave10, wave11, wave12, wave13, wave14, wave15, wave16, wave17, wave18, wave19, wave20 };

        foreach (Wave wave in allWaves)
        {
            waves.Add(wave);
        }
    }
}
