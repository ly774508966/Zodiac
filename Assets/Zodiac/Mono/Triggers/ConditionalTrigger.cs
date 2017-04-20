using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace Zodiac.Mono
{
	public class ConditionalTrigger : TriggerMono
	{
		
		public MemberMono condition;

		public List<ZodiacMono> IfFalse;



		public override void Connect (IEnumerable<ZodiacMono> selected)
		{
			
			//if (_fireCondition && !IfTrue.Contains (_event)) {
			//	IfTrue.Add (_event);
			//} else if (!IfFalse.Contains (_event)) {
			//	IfFalse.Add (_event);
			//}
		}

		public override void Fire ()
		{
			if (condition.GetRelationalResult ()) {
				ListFire (IfTrue);
			} else {
				ListFire (IfFalse);
			}
		}



	}

}