using UnityEngine;

public class HitboxManager : MonoBehaviour 
{
    Hitbox[] hitboxes;

    private void Start() {
        hitboxes = GetComponentsInChildren<Hitbox>();
    }
}