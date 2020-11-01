/************************************************************************************
* Copyright (c) 2020 quanljh@gmail.com All Rights Reserved.
* Author        :  quanljh
* NameSpace     :  Quan.Mapper
* FileName      :  QuanMapperProfile
* CreateTime    :  3/18/2020 10:32:35 PM
************************************************************************************/

using AutoMapper;
using Quan.Word.Core;

namespace Quan.Word
{
    /// <summary>
    ///
    /// </summary>
    public class QuanMapperProfile : Profile
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public QuanMapperProfile()
        {
            CreateMap<PatientUIModel, PatientUIModel>().ReverseMap();
            CreateMap<UserProfileDetailsApiModel, LoginCredentialsDataModel>().ReverseMap();
        }

        #endregion
    }
}
