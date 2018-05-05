CREATE TABLE [dbo].[tblTeam] (
    [TeamId]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (255)    NOT NULL,
    [NickName]    VARCHAR (255)    NULL,
    [Address1]    VARCHAR (255)    NULL,
    [Address2]    VARCHAR (255)    NULL,
    [Address3]    VARCHAR (255)    NULL,
    [Address4]    VARCHAR (255)    NULL,
    [Postcode]    VARCHAR (255)    NULL,
    [Colours]     VARCHAR (255)    NULL,
    [PhoneNumber] VARCHAR (255)    NULL,
    [Website]     VARCHAR (255)    NULL,
    [StadiumID] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_tblteam] PRIMARY KEY CLUSTERED ([TeamId] ASC), 
    CONSTRAINT [FK_tblTeam_ToStadium] FOREIGN KEY (StadiumID) REFERENCES [tblStadium](StadiumID)
);

