using UnityEngine;

public abstract class Behavior : MonoBehaviour
{
    [SerializeField] // Como ainda não temos nenhum singleton ou tag do player, insiro a ref do player manualmente
    protected Transform player;
    public abstract void Tick();
}