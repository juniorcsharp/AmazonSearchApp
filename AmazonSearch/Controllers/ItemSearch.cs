using AmazonProductAdvtApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using AmazonSearch.Models;
using System.IO;

namespace UptimeTestUlesanne.Controllers
{
    static class ItemSearch
    {

        private const string MY_AWS_ACCESS_KEY_ID = "MY_AWS_ACCESS_KEY_ID";
        private const string MY_AWS_SECRET_KEY = "MY_AWS_SECRET_KEY";
        private const string DESTINATION = "ecs.amazonaws.com";

        public static string ItemSearchRequest(string keywords, int page)
        {
            SignedRequestHelper helper = new SignedRequestHelper(MY_AWS_ACCESS_KEY_ID, MY_AWS_SECRET_KEY, DESTINATION);

            String requestUrl;

            String requestString = "Service=AWSECommerceService"
                   + "&Version=2009-03-31"
                   + "&Operation=ItemSearch"
                   + "&AssociateTag=SomeAsosiateTag"
                   + "&SearchIndex=All"
                   + "&Keywords=" + keywords
                   + "&ItemPage=" + page
                   + "&ResponseGroup=Request,ItemAttributes,OfferSummary,Images,Reviews,EditorialReview"
                   ;
            requestUrl = helper.Sign(requestString);


            return requestUrl;
        }

        public static List<SearchedItem> ItemSearchResponse(string url)
        {
            List<SearchedItem> searchedItems = new List<SearchedItem>();

            WebRequest request = HttpWebRequest.Create(url);

            try

            {
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();

                XmlDocument doc = new XmlDocument();

                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

                doc.LoadXml(reader.ReadToEnd());

                XmlNodeList listIsValid = doc.GetElementsByTagName("IsValid");

                if (listIsValid.Count > 0 && listIsValid[0].InnerXml == "True")
                {
                    XmlNodeList listItems = doc.GetElementsByTagName("Item");
                    foreach (XmlNode nodeItem in listItems)
                    {
                        SearchedItem searchedItem = new SearchedItem();

                        foreach (XmlNode nodeChild in nodeItem.ChildNodes)
                        {
                            if (nodeChild.Name == "DetailPageURL")
                            {
                                searchedItem.DetailPageURL = nodeChild.InnerText;
                            }

                            else if (nodeChild.Name == "SmallImage")
                            {
                                foreach (XmlNode nodeURLImg in nodeChild.ChildNodes)
                                {
                                    if (nodeURLImg.Name == "URL")
                                    {
                                        searchedItem.SmallImage = nodeURLImg.InnerText;
                                    }
                                }
                            }

                            else if (nodeChild.Name == "ItemAttributes")
                            {
                                foreach (XmlNode nodeItemAtributes in nodeChild.ChildNodes)
                                {
                                    if (nodeItemAtributes.Name == "Title")
                                    {
                                        searchedItem.Title = nodeItemAtributes.InnerText;
                                    }
                                }
                            }

                            else if (nodeChild.Name == "OfferSummary")
                            {
                                foreach (XmlNode nodeOfferSummary in nodeChild.ChildNodes)
                                {
                                    if (nodeOfferSummary.Name == "LowestNewPrice")
                                    {
                                        foreach (XmlNode nodeLowestNewPrice in nodeOfferSummary.ChildNodes)
                                        {
                                            if (nodeLowestNewPrice.Name == "CurrencyCode")
                                            {
                                                searchedItem.CurrencyCode = nodeLowestNewPrice.InnerText;
                                            }

                                            else if (nodeLowestNewPrice.Name == "FormattedPrice")
                                            {
                                                string price = nodeLowestNewPrice.InnerText.Remove(0, 1);
                                                searchedItem.NewPrice = Double.Parse(price);
                                                break;
                                            }
                                        }
                                    }

                                    else if (nodeOfferSummary.Name == "LowestUsedPrice")
                                    {
                                        foreach (XmlNode nodeLowestUsedPrice in nodeOfferSummary.ChildNodes)
                                        {
                                            if (nodeLowestUsedPrice.Name == "CurrencyCode")
                                            {
                                                searchedItem.CurrencyCode = nodeLowestUsedPrice.InnerText;
                                            }

                                            else if (nodeLowestUsedPrice.Name == "FormattedPrice")
                                            {
                                                string price = nodeLowestUsedPrice.InnerText.Remove(0, 1);
                                                searchedItem.UsedPrice = Double.Parse(price);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            else if (nodeChild.Name == "CustomerReviews")
                            {
                                foreach (XmlNode nodeCustomerReview in nodeChild.ChildNodes)
                                {
                                    if (nodeCustomerReview.Name == "IFrameURL")
                                    {
                                        searchedItem.CustomerReviewURL = nodeCustomerReview.InnerText;
                                        break;
                                    }
                                }
                            }
                        }
                        searchedItems.Add(searchedItem);
                    }
                }
                return searchedItems;
            }


            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
    }
}