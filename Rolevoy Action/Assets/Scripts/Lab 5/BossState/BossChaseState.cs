using UnityEngine;

public class BossChaseState : IState
{
    private EnemyContext context;
    private EnemyStateMachineAI ai;

    public BossChaseState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public void Enter()
    {
        context.agent.speed = context.chaseSpeed;
        context.agent.isStopped = false;
        context.animator.Iswalking(false);
        context.animator.Isrunning(true);
    }

    public void Update()
    {
        float dist = context.DistanceToPlayer;

        if (dist <= context.attackRange)
        {
            // ѕереход в атаку
            ai.GetStateMachine().ChangeState(new BossAttackState(context, ai));
            return;
        }

        if (dist > context.stopChaseRange)
        {
            // —лишком далеко Ц возврат в Idle (но босс уже заагрен, можно оставить Chase или вернуть в Idle)
            // ѕо заданию: босс не должен тер€ть агрессию, поэтому оставл€ем Chase, но можно и вернуть в Idle.
            // ќставим Chase, но остановим движение. ƒл€ простоты Ц продолжаем сто€ть.
            context.agent.isStopped = true;
            return;
        }

        context.agent.isStopped = false;
        context.agent.SetDestination(context.player.position);
        RotateTowardsPlayer();
    }

    public void Exit()
    {
        context.agent.isStopped = false;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dir = (context.player.position - context.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
}