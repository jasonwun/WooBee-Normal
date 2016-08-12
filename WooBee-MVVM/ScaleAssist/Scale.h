#pragma once

namespace DX
{
	inline void ThrowIfFailed(HRESULT hr)
	{
		if (FAILED(hr))
		{
			// Set a breakpoint on this line to catch DirectX API errors
			throw Platform::Exception::CreateException(hr);
		}
	}
}

namespace ScaleAssist
{
	public ref class Scale sealed
	{
	public:
		Scale();
		property Windows::Foundation::Point ScreenResolution
		{
			Windows::Foundation::Point get();
		}

	private:
		D3D_FEATURE_LEVEL							    m_featureLevel;
		Microsoft::WRL::ComPtr<ID3D11Device1>           m_d3dDevice;
	};
}
