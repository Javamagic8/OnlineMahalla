using System;

using Com.Intsoft.Crypto;

namespace Com.Intsoft.Crypto.Parameters
{
	public class UZDST1092KeyParameters : AsymmetricKeyParameter
	{
		private readonly UZDST1092Parameters parameters;
		protected UZDST1092KeyParameters(
			bool				isPrivate,
			UZDST1092Parameters parameters)
			: base(isPrivate)
		{
			this.parameters = parameters;
		}

		public UZDST1092Parameters GetParameters() 
		{
			return parameters;
		}

	}
}
