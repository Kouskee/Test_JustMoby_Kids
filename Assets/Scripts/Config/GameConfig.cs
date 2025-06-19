using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Cubes")]
    [SerializeField] private List<Sprite> _availableCubes;
    [SerializeField] private int _scrollCubesCount = 20;

    [Header("Tower")]
    [SerializeField] private float _maxHorizontalOffset = 0.5f;
    [SerializeField] private float _cubeSize = 50f;

    public List<Sprite> AvailableCubes => _availableCubes;
    public int ScrollCubesCount => _scrollCubesCount;
    public float MaxHorizontalOffset => _maxHorizontalOffset;
    public float CubeSize => _cubeSize;
}