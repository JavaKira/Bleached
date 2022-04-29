using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private AnimationCurve jumpCurve;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) TryJump();
        if (Input.GetKey(KeyCode.D)) Move(Vector2.right);
        if (Input.GetKey(KeyCode.A)) Move(Vector2.left);
    }

    private void Move(Vector2 direction)
    {
        var scale = transform.localScale;
        if (direction.x > 0)
            transform.localScale = new Vector3(1, scale.y, scale.z);

        if (direction.x < 0)
            transform.localScale = new Vector3(-1, scale.y, scale.z);
        
        transform.Translate(direction.normalized * (speedMultiplier * Time.deltaTime));
    }

    private void TryJump()
    {
        if (!IsGrounded())
            return;

        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        int frame = default;
        while (frame < jumpCurve.length)
        {
            transform.Translate(0, jumpCurve[frame].value * jumpMultiplier * Time.fixedDeltaTime, 0);
            yield return new WaitForFixedUpdate();
            frame++;
        }

        yield return null;
    }

    private bool IsGrounded()
    {
        var position = transform.position;
        var hit = Physics2D.Raycast(new Vector2(position.x, position.y - 0.8f), Vector2.down, 1f);
        if (hit.collider == null) return false;
        return hit.distance == 0;
    }
}