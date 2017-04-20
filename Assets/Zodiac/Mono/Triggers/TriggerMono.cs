using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace Zodiac.Mono
{
	public class TriggerMono : ZodiacMono
	{

		public List<ZodiacMono> IfTrue;

		public LinkRenderer ifTrueLinker;

		public bool fire;

		public override void Init ()
		{
			base.Init ();
			ifTrueLinker = LinkRenderer.AddLinkRenderer (this, LinkerType.Trigger);
			IfTrue = new List<ZodiacMono> ();
		}

		protected override void OnValidate ()
		{
			base.OnValidate ();
			if (fire) {
				Fire ();
				fire = !fire;
			}
		}

		protected override void Update ()
		{
			ifTrueLinker.Show<ZodiacMono> (IfTrue);
			base.Update ();
		}

		public override void Connect (IEnumerable<ZodiacMono> _selected)
		{
			IfTrue.AddRange (_selected.Where (s => !IfTrue.Contains (s)));

		}

		public virtual void Fire ()
		{
			ListFire (IfTrue);
		}

		protected void ListFire (IEnumerable<ZodiacMono> elements)
		{
			foreach (var obj in elements) {
				if (obj is TriggerMono) {
					((TriggerMono)obj).Fire ();
				} else if (obj is MemberMono) {
					((MemberMono)obj).GetBoxedData ();
				}
			}
		}



	}

}