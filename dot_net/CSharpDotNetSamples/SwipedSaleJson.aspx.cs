﻿using System;
using System.Web;
using System.Web.UI;

namespace AspNetClientEncryptionExample
{
	
	public partial class SwipedSaleJson : System.Web.UI.Page
	{
		public void BtnSwipedSaleClicked(object sender, EventArgs args)
		{
			if(this.IsPostBack) 
			{
				OAuthTokenGenerator tokenGenerator = new OAuthTokenGenerator ();
				IsOAuthTokenSuccessful(tokenGenerator.GetToken ());
			}
		}


		protected void IsOAuthTokenSuccessful(OAuthToken OAuthResult) 
		{

			if(OAuthResult.errorflag == false)
			{
				// In case of not using any OAuth2.0 Library
				// Use following when OAuth2.0 is caseinsesitive at Paytrace. 
				// string OAuth = String.Format ("{0} {1}", OAuthResult.token_type, OAuthResult.access_token);
				// For now OAuth2.0 is not caseinsesitive at PayTrace - ESC-141

				string OAuth = String.Format ("Bearer {0}", OAuthResult.access_token);
			
				// Swiped Sale Request
				SwipedSaleRequest swipeSaleRequest = new SwipedSaleRequest();

				//Swiped Sale Transaction
				SwipedSaleGenerator swipedSaleGenerator = new SwipedSaleGenerator();

				//Assign the values to the Swiped Sale Request.
				swipeSaleRequest = BuildRequestFromFields(swipeSaleRequest);

				//Swiped Sale Request and display the result
				var result = swipedSaleGenerator.SwipedSaleTrans(OAuth,swipeSaleRequest);

				//process the Swiped Sale response
				WriteResults(result);

			} 
			else // Error for OAuth
			{
				// Do your own code here to handle the error

				// Display the OAuth Error - Optional
				Response.Write (" Http Status Code & Description : " +  OAuthResult.Error.token_error_http  + "<br>");
				Response.Write (" API Error : " +  OAuthResult.Error.error + "<br>");
				Response.Write (" API Error Message : " +  OAuthResult.Error.error_description+ "<br>");
				Response.Write (" Token Request: " + "Failed!" + "<br>");
			
			}

		}

		protected SwipedSaleRequest BuildRequestFromFields(SwipedSaleRequest requestSwipedSale) 
		{
			// Build Keyed Sale Request fields from the input source
			requestSwipedSale.amount = 5.00;

			//Swipe value should be detected from Magnetic stripe reader(Credit Card reader Device) 
			requestSwipedSale.swipe = "%B4012881888818888^Demo/Customer^2412101001020001000000701000000?;4012881888818888=24121010010270100001?";

			return requestSwipedSale;

		}

		protected void WriteResults(PayTraceBasicSaleResponse result) 
		{

			if(null != result.ErrorMsg  && result.success == false )
			{
				Response.Write (" Http Error Code & Error : " + result.ErrorMsg + "<br>");

				Response.Write ("Success : " + result.success + "<br>"); 
				Response.Write ("response_code : " + result.response_code + "<br>");   
				Response.Write ("status_message : " + result.status_message + "<br>"); 
				Response.Write ("external_transaction_id : " + result.external_transaction_id + "<br>"); 

				// Check the actual API errors with appropriate code
				Response.Write (" API errors : "+ "<br>");
				foreach (var item in result.errors) 
				{
					foreach (var errorMessage in (string[])item.Value) // to read Error message with each code in array.
					{
						Response.Write (item.Key  + "=" + errorMessage + "<BR>");
					}
				}
				Response.Write ("Swiped Sale : " + "Failed!" + "<br>");	
			} 
			else
			{
				// Do your code here when Response is available based on the response_code. 
				// Please refer PayTrace-HTTP Status and Error Codes page for possible errors and Response Codes
				// for transation successfully approved 
				if (result.response_code == 101 && result.success == true ) 
				{
					// Do you code for any additional verification

					// Display Response - optional
					DisplaySaleResponse(result);
					Response.Write ("Swiped Sale : " + "Success!" + "<br>");		

				}
				else
				{
					// Do your code here based on the response_code - use the PayTrace http status and error page for reference
					// Do your code for any additional verifications - avs_response and csc_response

					//Display Response - optionals
					DisplaySaleResponse(result);
					Response.Write ("Error : " + result.ErrorMsg + "<br>");

				}
				// Do your code for Any additional task !
			}
		}

		// Display the Swiped Sale Response
		protected void DisplaySaleResponse(PayTraceBasicSaleResponse result)
		{

			Response.Write ("Success : " + result.success + "<br>"); 
			Response.Write ("response_code : " + result.response_code + "<br>");   
			Response.Write ("status_message : " + result.status_message + "<br>"); 
			Response.Write ("transaction_id : " + result.transaction_id + "<br>"); 
			Response.Write ("approval_code : " + result.approval_code + "<br>"); 
			Response.Write ("approval_message : " + result.approval_message + "<br>"); 
			Response.Write ("avs_response : " + result.avs_response + "<br>"); 
			Response.Write ("csc_response : " + result.csc_response + "<br>"); 
			Response.Write ("external_transaction_id : " + result.external_transaction_id + "<br>"); 

		}


	}


}

