using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour {
  public Color waterColor;
  public MeshRenderer meshRenderer;

  private void Start() {
    meshRenderer.material.SetColor("_Color", waterColor);
  }
}
