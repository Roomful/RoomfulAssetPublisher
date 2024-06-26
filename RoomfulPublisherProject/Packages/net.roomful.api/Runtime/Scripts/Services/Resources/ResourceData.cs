using System;
using System.Collections.Generic;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    public class ResourceData
    {
        private DataDirectory m_dataDirectory;
        private DataAmazon m_dataAmazon;
        private RemoteUrlData m_remoteUrl;

        public ResourceData(Dictionary<string, object> serverParams, Dictionary<string, object> resourceParams) {
            ParseData(serverParams, resourceParams);
        }

        private void ParseData(Dictionary<string, object> serverParams,  IDictionary<string, object> resourceParams) {
            foreach (var temp in serverParams) {
                ResourceEnum.Data type;
                if (Enum.TryParse(temp.Key, true, out type)) {
                    var serverValues = temp.Value as Dictionary<string, object>;
                    switch (type) {
                        case ResourceEnum.Data.Audio:
                            break;
                        case ResourceEnum.Data.Video:
                            break;
                        case ResourceEnum.Data.Amazon:
                            m_dataAmazon = new DataAmazon();
                            foreach (var pair in serverValues) {
                                if (pair.Key.Equals("url")) {
                                    if (!resourceParams.ContainsKey(ResourceEnum.ParamsKeys.weblink.ToString())) {
                                        resourceParams.Add(ResourceEnum.ParamsKeys.weblink.ToString(), pair.Value.ToString());
                                    }

                                    resourceParams[ResourceEnum.ParamsKeys.weblink.ToString()] = pair.Value.ToString();
                                    m_dataAmazon.Url = resourceParams[ResourceEnum.ParamsKeys.weblink.ToString()].ToString();
                                }

                                if (pair.Key.Equals("summary")) {
                                    if (!resourceParams.ContainsKey(ResourceEnum.ParamsKeys.weblinkdescription.ToString())) {
                                        resourceParams.Add(ResourceEnum.ParamsKeys.weblinkdescription.ToString(), pair.Value.ToString());
                                    }

                                    resourceParams[ResourceEnum.ParamsKeys.weblinkdescription.ToString()] = pair.Value.ToString();
                                    m_dataAmazon.Summary = resourceParams[ResourceEnum.ParamsKeys.weblinkdescription.ToString()].ToString();
                                }

                                if (pair.Key.Equals("author")) {
                                    var authors = (List<object>) Convert.ChangeType(pair.Value, typeof(List<object>));
                                    authors.ForEach(author => {
                                        var resId = Convert.ToString(author);
                                        m_dataAmazon.Author.Add(resId);
                                    });
                                    if (!resourceParams.ContainsKey(ResourceEnum.ParamsKeys.author.ToString())) {
                                        resourceParams.Add(ResourceEnum.ParamsKeys.author.ToString(), m_dataAmazon.Author);
                                    }

                                    resourceParams[ResourceEnum.ParamsKeys.author.ToString()] = m_dataAmazon.Author;
                                }
                            }

                            break;
                        case ResourceEnum.Data.Imdb:
                            break;
                        case ResourceEnum.Data.Youtube:
                            break;
                        case ResourceEnum.Data.Pinterest:
                            break;
                        case ResourceEnum.Data.Pixabay:
                            break;
                        case ResourceEnum.Data.Facebook:
                            break;
                        case ResourceEnum.Data.RemoteUrl:
                            m_remoteUrl = RemoteUrlData.FromJsonData(serverValues);
                            break;
                        case ResourceEnum.Data.Directory:
                            m_dataDirectory = new DataDirectory();
                            foreach (var pair in serverValues) {
                                if (pair.Key.Equals("innerContentType")) {
                                    DataDirectory.DirectoryType innerContentType;
                                    if (Enum.TryParse(pair.Value.ToString(), true, out innerContentType)) {
                                        m_dataDirectory.Type = innerContentType;
                                    }
                                    else {
                                        Debug.LogError("{0} is not a member of the ResourceData DirectoryType enumeration.");
                                    }
                                }

                                if (pair.Key.Equals("innerContentCount")) {
                                    m_dataDirectory.InnerContentCount = Convert.ToInt32(pair.Value);
                                }
                            }

                            break;
                    }
                }
                else {
                    Debug.LogError("{0} is not a member of the Resource Data enumeration.");
                }
            }
        }

        public DataDirectory GetDataDirectory => m_dataDirectory;

        public DataAmazon GetDataAmazon => m_dataAmazon;

        public RemoteUrlData RemoteUrl => m_remoteUrl;

        [Serializable]
        public class DataDirectory
        {
            public enum DirectoryType
            {
                Empty,
                Mixed,
                ImagesOrVideo,
                OnlyBooks
            }

            public DirectoryType Type { get; set; }
            public int InnerContentCount { get; set; }
        }

        [Serializable]
        public class DataAmazon
        {
            public string Asin { get; set; } = string.Empty;
            public string Summary { get; set; } = string.Empty;
            public List<string> Author { get; } = new List<string>();
            public string Manufacturer { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string PublicationDate { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
        }

        [Serializable]
        public class RemoteUrlData
        {
            public string Url { get; set; }
            public string UrlType { get; set; }
            public string FavIcon { get; set; }
            public string Title { get; set; }

            internal static RemoteUrlData FromJsonData(Dictionary<string, object> jsonData) {
                var data = new RemoteUrlData {
                    Url = (string)jsonData["url"],
                    UrlType = (string)jsonData["urlType"],
                    FavIcon = (string)jsonData["favicon"],
                    Title = (string)jsonData["title"]
                };

                return data;
            }
        }
    }
}