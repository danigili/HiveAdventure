using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if (UNITY_EDITOR) 
public class LevelEditor : MonoBehaviour
{
    public TextAsset level;

    public CameraController camera;
    public Toggle levelTypeToggle;
    public InputField maxTurnsInputField;

    public Pool piecesPool;
    public Pool markersPool;
    public Pool rocksPool;
    public Pool chainsPool;

    public PiecesPanel[] panels;
    public Dropdown dropdown;

    private Board model;

    private bool editMode = true;
    public HexObject selectedItem;
    public PieceUI selectedUIPiece;

    // Start is called before the first frame update
    void Start()
    {
        selectedItem = null;
        dropdown.options.Clear();
        for (int i = 0; i < 2; i++)
        {
            foreach (BugType bug in Enum.GetValues(typeof(BugType)))
            {
                Dropdown.OptionData option1 = new Dropdown.OptionData();
                option1.text = ( i == 0 ? "White" : "Black" ) + " " + bug.ToString();
                dropdown.options.Add(option1);
            }
        }
        Dropdown.OptionData option = new Dropdown.OptionData();
        option.text = "(+) Add piece...";
        dropdown.options.Add(option);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }
    private void UpdateCameraPosition()
    {
        if (piecesPool.Count() > 0)
        {
            float xMin, xMax, yMin, yMax;
            BoardSize(out xMin, out xMax, out yMin, out yMax);
            camera.SetCenter((xMin + xMax) / 2, (yMin + yMax) / 2);
            camera.SetSize(Mathf.Max(Mathf.Max(xMax - xMin, yMax - yMin) * 0.5f + 1f, +5));
        }
        else
        {
            camera.SetCenter(1.558845f, 0);
            camera.SetSize(5);
        }
        camera.SetAngle(60);
    }

    // Callbacks

    public void LoadLevel()
    {
        piecesPool.ClearAll();
        markersPool.ClearAll();
        rocksPool.ClearAll();
        chainsPool.ClearAll();

        model = Serializer<Board>.FromJson(level.text);

        foreach (KeyValuePair<(int, int, int), Piece> pair in model.GetPlacedPieces())
        {
            GameObject instance = piecesPool.GetInstance(true);
            instance.GetComponent<PieceObject>().Initialize(pair.Value, ClickDown);
            instance.GetComponent<PieceObject>().x = pair.Key.Item1;
            instance.GetComponent<PieceObject>().y = pair.Key.Item2;
            instance.GetComponent<PieceObject>().z = pair.Key.Item3;
            if (pair.Value.blocked)
            {
                GameObject chainInstance = chainsPool.GetInstance(true);
                chainInstance.GetComponent<HexObject>().SetHexPosition(pair.Value.position);
            }
        }

        foreach (Position p in model.GetBlockedPositions())
        {
            GameObject instance = rocksPool.GetInstance(true);
            instance.GetComponent<RockObject>().Initialize(ClickDown);
            instance.GetComponent<HexObject>().SetHexPosition(p);
        }

        panels[0].Initialize(model, ClickPanelPiece);
        panels[1].Initialize(model, ClickPanelPiece);
        panels[0].transform.parent.GetComponent<Animator>().SetBool("show", true);

        selectedItem = null;
        selectedUIPiece = null;

    }

    public void SaveLevel()
    {
        File.WriteAllText(AssetDatabase.GetAssetPath(level), Serializer<Board>.ToJson(model));
        AssetDatabase.Refresh();// ..AddObjectToAsset(level, AssetDatabase.GetAssetPath(level));
        Debug.Log(Serializer<Board>.ToJson(model));
    }

    public void PlayButton()
    { 
    
    }

    public void AddRock() 
    {
        selectedItem = rocksPool.GetInstance<RockObject>(false);
        ((RockObject)selectedItem).Initialize(ClickDown);
        for (int i = -4; i < 4; i++)
        {
            for (int j = -8; j < 8; j++)
            {
                Marker marker = markersPool.GetInstance<Marker>(true);
                marker.SetHexPosition(i, j, 0);
                marker.Initialize(ClickDown);
            }
        }
    }

    public void AddChain()
    {
        markersPool.ClearAll();
        selectedItem = chainsPool.GetInstance<ChainObject>(false);
    }

    public void AddPiece(Dropdown dropdown)
    {
        if (dropdown.value == dropdown.options.Count - 1)
            return;
        model.CreateNewPiece(dropdown.value < 5 ? false : true, (BugType)(dropdown.value % 5));
        dropdown.value = dropdown.options.Count - 1;
        panels[0].Initialize(model, ClickPanelPiece);
        panels[1].Initialize(model, ClickPanelPiece);
    }

    public void RemovePiece()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetType() == typeof(RockObject))
            {
                model.GetBlockedPositions().Remove(selectedItem.GetHexPosition());
                selectedItem.gameObject.SetActive(false);
            }

            else if (selectedItem.GetType() == typeof(PieceObject))
            {
                if (((PieceObject)selectedItem).piece.blocked)
                {
                    ((PieceObject)selectedItem).piece.blocked = false;
                    foreach (GameObject obj in chainsPool)
                    {
                        if (obj.GetComponent<ChainObject>().GetHexPosition() == selectedItem.GetHexPosition())
                            obj.SetActive(false);
                    }
                    selectedItem = null;
                }
                else
                {
                    model.GetPlacedPieces().Remove((selectedItem.GetHexPosition().x, selectedItem.GetHexPosition().y, selectedItem.GetHexPosition().z));
                    selectedItem.gameObject.SetActive(false);
                }
            }
        }
        else if (selectedUIPiece != null)
        {
            model.GetNotPlacedPieces().Remove(selectedUIPiece.piece);
            panels[0].RemovePiece(selectedUIPiece);
            panels[1].RemovePiece(selectedUIPiece);
        }
    }

    public void ClickPanelPiece(PieceUI piece)
    {
        selectedUIPiece = piece;
        selectedItem = null;
        markersPool.ClearAll();
        for (int i = -4; i < 4; i++)
        {
            for (int j = -8; j < 8; j++)
            {
                Marker marker = markersPool.GetInstance<Marker>(true);
                marker.SetHexPosition(i, j, 0);
                marker.Initialize(ClickDown);
            }
        }
    }

    public void ClickDown(HexObject piece)
    {
        if (typeof(Marker) == piece.GetType())
        {
            // Add the rock to the board and place object
            RockObject tmp;
            if (selectedItem != null)
            {
                if (selectedItem.TryGetComponent<RockObject>(out tmp))
                {
                    model.GetBlockedPositions().Add(piece.GetHexPosition());
                    selectedItem.SetHexPosition(piece);
                    selectedItem.gameObject.SetActive(true);
                    selectedItem = null;
                    markersPool.ClearAll();
                }
            }
            else if (selectedUIPiece != null)
            {
                model.GetNotPlacedPieces().Remove(selectedUIPiece.piece);
                model.GetPlacedPieces().Add((piece.x, piece.y, piece.z), selectedUIPiece.piece);
                panels[0].RemovePiece(selectedUIPiece);
                panels[1].RemovePiece(selectedUIPiece);
                markersPool.ClearAll();
                GameObject instance = piecesPool.GetInstance(true);
                instance.GetComponent<PieceObject>().Initialize(selectedUIPiece.piece, ClickDown);
                instance.GetComponent<PieceObject>().SetHexPosition(piece);
                selectedUIPiece = null;
            }
        }
        else if (typeof(RockObject) == piece.GetType())
        {
            markersPool.ClearAll();
            selectedItem = piece;
        }
        else if (typeof(PieceObject) == piece.GetType())
        {
            if (selectedItem != null)
            {
                // Add a chain to the piece
                if (selectedItem.GetComponent<ChainObject>() != null)
                {
                    ((PieceObject)piece).piece.blocked = true;
                    selectedItem.SetHexPosition(piece);
                    selectedItem.gameObject.SetActive(true);
                    selectedItem = null;
                    return;
                }
            }
            // Select piece
            selectedItem = piece;
        }
    }

    public void EndOfGame(Winner winner)
    { 
        
    }

    // TODO refactorize
    public void BoardSize(out float xMin, out float xMax, out float yMin, out float yMax)
    {
        xMin = 1000;
        xMax = -1000;
        yMin = 1000;
        yMax = -1000;

        foreach (PieceObject p in piecesPool.Next<PieceObject>())
        {
            xMin = Math.Min(xMin, p.transform.position.x);
            xMax = Math.Max(xMax, p.transform.position.x);
            yMin = Math.Min(yMin, p.transform.position.z);
            yMax = Math.Max(yMax, p.transform.position.z);
        }
    }
}
#endif