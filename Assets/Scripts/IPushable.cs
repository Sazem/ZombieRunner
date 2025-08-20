using UnityEngine;

public interface IPushable
{
    void ReceivePush(Vector3 direction, float pushForce);
}
