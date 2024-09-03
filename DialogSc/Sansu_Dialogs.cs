using UnityEngine;

[RequireComponent(typeof(Sansu_DialogSystem))]
public class Sansu_Dialogs : Sansu_DialogBase
{
	// 캐릭터들의 대사를 진행하는 DialogSystem
	private Sansu_DialogSystem dialogSystem;

	public override void Enter()
	{
		dialogSystem = GetComponent<Sansu_DialogSystem>();
		dialogSystem.Setup();
	}

	public override void Execute(Sansu_DialogController controller)
	{
		// 현재 분기에 진행되는 대사 진행
		bool isCompleted = dialogSystem.UpdateDialog();

		// 현재 분기의 대사 진행이 완료되면
		if ( isCompleted == true )
		{
			// 다음 튜토리얼로 이동
			controller.SetNextDialog();
		}
	}

	/*
	public override void Exit()
	{
	}
	*/
}

