IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
    BEGIN
        CREATE TABLE [__EFMigrationsHistory] (
            [MigrationId]    NVARCHAR (150) NOT NULL,
            [ProductVersion] NVARCHAR (32)  NOT NULL,
            CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
        );
    END


GO
INSERT  INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES                              (N'20260203144144_Initial', N'10.0.2');


GO
IF SCHEMA_ID(N'placeholder') IS NULL
    EXECUTE (N'CREATE SCHEMA [placeholder];');


GO
CREATE TABLE [placeholder].[country] (
    [country_id]   UNIQUEIDENTIFIER NOT NULL,
    [country_code] CHAR (2)         NOT NULL,
    [country_name] NVARCHAR (150)   NOT NULL,
    [country_type] VARCHAR (6)      NOT NULL,
    [contest_ids]  NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_country] PRIMARY KEY CLUSTERED ([country_id]),
    CONSTRAINT [AK_country_country_code] UNIQUE ([country_code]),
    CONSTRAINT [CK_country_country_type_Enum] CHECK ([country_type] IN ('Real', 'Pseudo'))
);


GO
INSERT  INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES                              (N'20260204060648_Create_placeholder_country_aggregate_table', N'10.0.2');


GO
CREATE TABLE [placeholder].[contest] (
    [contest_id]                        UNIQUEIDENTIFIER NOT NULL,
    [contest_year]                      INT              NOT NULL,
    [city_name]                         NVARCHAR (150)   NOT NULL,
    [semi_final_voting_format]          VARCHAR (15)     NOT NULL,
    [grand_final_voting_format]         VARCHAR (15)     NOT NULL,
    [queryable]                         BIT              NOT NULL,
    [global_televote_voting_country_id] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_contest] PRIMARY KEY CLUSTERED ([contest_id]),
    CONSTRAINT [AK_contest_contest_year] UNIQUE ([contest_year]),
    CONSTRAINT [CK_contest_contest_year] CHECK ([contest_year] BETWEEN 2016 AND 2050),
    CONSTRAINT [CK_contest_grand_final_voting_format_Enum] CHECK ([grand_final_voting_format] IN ('TelevoteAndJury', 'TelevoteOnly')),
    CONSTRAINT [CK_contest_semi_final_voting_format_Enum] CHECK ([semi_final_voting_format] IN ('TelevoteAndJury', 'TelevoteOnly'))
);


GO
CREATE TABLE [placeholder].[contest_broadcast_memo] (
    [row_id]        INT              IDENTITY NOT NULL,
    [broadcast_id]  UNIQUEIDENTIFIER NOT NULL,
    [contest_stage] VARCHAR (10)     NOT NULL,
    [completed]     BIT              NOT NULL,
    [contest_id]    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_contest_broadcast_memo] PRIMARY KEY CLUSTERED ([row_id]),
    CONSTRAINT [CK_contest_broadcast_memo_contest_stage_Enum] CHECK ([contest_stage] IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')),
    CONSTRAINT [FK_contest_broadcast_memo_contest_contest_id] FOREIGN KEY ([contest_id]) REFERENCES [placeholder].[contest] ([contest_id]) ON DELETE CASCADE
);


GO
CREATE TABLE [placeholder].[contest_participant] (
    [participating_country_id] UNIQUEIDENTIFIER NOT NULL,
    [contest_id]               UNIQUEIDENTIFIER NOT NULL,
    [semi_final_draw]          VARCHAR (10)     NOT NULL,
    [act_name]                 NVARCHAR (150)   NOT NULL,
    [song_title]               NVARCHAR (150)   NOT NULL,
    CONSTRAINT [PK_contest_participant] PRIMARY KEY CLUSTERED ([contest_id], [participating_country_id]),
    CONSTRAINT [CK_contest_participant_semi_final_draw_Enum] CHECK ([semi_final_draw] IN ('SemiFinal1', 'SemiFinal2')),
    CONSTRAINT [FK_contest_participant_contest_contest_id] FOREIGN KEY ([contest_id]) REFERENCES [placeholder].[contest] ([contest_id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE INDEX [IX_contest_broadcast_memo_contest_id_broadcast_id]
    ON [placeholder].[contest_broadcast_memo]([contest_id], [broadcast_id]);


GO
INSERT  INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES                              (N'20260204062914_Create_placeholder_contest_aggregate_tables', N'10.0.2');


GO
CREATE TABLE [placeholder].[broadcast] (
    [broadcast_id]      UNIQUEIDENTIFIER NOT NULL,
    [broadcast_date]    DATE             NOT NULL,
    [parent_contest_id] UNIQUEIDENTIFIER NOT NULL,
    [contest_stage]     VARCHAR (10)     NOT NULL,
    [voting_format]     VARCHAR (15)     NOT NULL,
    [completed]         BIT              NOT NULL,
    CONSTRAINT [PK_broadcast] PRIMARY KEY CLUSTERED ([broadcast_id]),
    CONSTRAINT [AK_broadcast_broadcast_date] UNIQUE ([broadcast_date]),
    CONSTRAINT [AK_broadcast_parent_contest_id_contest_stage] UNIQUE ([parent_contest_id], [contest_stage]),
    CONSTRAINT [CK_broadcast_broadcast_date] CHECK ([broadcast_date] BETWEEN '2016-01-01' AND '2050-12-31'),
    CONSTRAINT [CK_broadcast_contest_stage_Enum] CHECK ([contest_stage] IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')),
    CONSTRAINT [CK_broadcast_voting_format_Enum] CHECK ([voting_format] IN ('TelevoteAndJury', 'TelevoteOnly'))
);


GO
CREATE TABLE [placeholder].[broadcast_competitor] (
    [competing_country_id] UNIQUEIDENTIFIER NOT NULL,
    [broadcast_id]         UNIQUEIDENTIFIER NOT NULL,
    [performing_spot]      INT              NOT NULL,
    [broadcast_half]       VARCHAR (6)      NOT NULL,
    [finishing_spot]       INT              NOT NULL,
    CONSTRAINT [PK_broadcast_competitor] PRIMARY KEY CLUSTERED ([broadcast_id], [competing_country_id]),
    CONSTRAINT [CK_broadcast_competitor_broadcast_half_Enum] CHECK ([broadcast_half] IN ('First', 'Second')),
    CONSTRAINT [CK_broadcast_competitor_finishing_spot] CHECK ([finishing_spot] >= 1),
    CONSTRAINT [CK_broadcast_competitor_performing_spot] CHECK ([performing_spot] >= 1),
    CONSTRAINT [FK_broadcast_competitor_broadcast_broadcast_id] FOREIGN KEY ([broadcast_id]) REFERENCES [placeholder].[broadcast] ([broadcast_id]) ON DELETE CASCADE
);


GO
CREATE TABLE [placeholder].[broadcast_jury] (
    [voting_country_id] UNIQUEIDENTIFIER NOT NULL,
    [broadcast_id]      UNIQUEIDENTIFIER NOT NULL,
    [points_awarded]    BIT              NOT NULL,
    CONSTRAINT [PK_broadcast_jury] PRIMARY KEY CLUSTERED ([broadcast_id], [voting_country_id]),
    CONSTRAINT [FK_broadcast_jury_broadcast_broadcast_id] FOREIGN KEY ([broadcast_id]) REFERENCES [placeholder].[broadcast] ([broadcast_id]) ON DELETE CASCADE
);


GO
CREATE TABLE [placeholder].[broadcast_televote] (
    [voting_country_id] UNIQUEIDENTIFIER NOT NULL,
    [broadcast_id]      UNIQUEIDENTIFIER NOT NULL,
    [points_awarded]    BIT              NOT NULL,
    CONSTRAINT [PK_broadcast_televote] PRIMARY KEY CLUSTERED ([broadcast_id], [voting_country_id]),
    CONSTRAINT [FK_broadcast_televote_broadcast_broadcast_id] FOREIGN KEY ([broadcast_id]) REFERENCES [placeholder].[broadcast] ([broadcast_id]) ON DELETE CASCADE
);


GO
CREATE TABLE [placeholder].[broadcast_competitor_points_award] (
    [row_id]               INT              IDENTITY NOT NULL,
    [voting_country_id]    UNIQUEIDENTIFIER NOT NULL,
    [voting_method]        VARCHAR (8)      NOT NULL,
    [points_value]         INT              NOT NULL,
    [broadcast_id]         UNIQUEIDENTIFIER NOT NULL,
    [competing_country_id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_broadcast_competitor_points_award] PRIMARY KEY CLUSTERED ([row_id]),
    CONSTRAINT [CK_broadcast_competitor_points_award_country_ids] CHECK ([competing_country_id] <> [voting_country_id]),
    CONSTRAINT [CK_broadcast_competitor_points_award_points_value] CHECK ([points_value] BETWEEN 0 AND 12),
    CONSTRAINT [CK_broadcast_competitor_points_award_voting_method_Enum] CHECK ([voting_method] IN ('Televote', 'Jury')),
    CONSTRAINT [FK_broadcast_competitor_points_award_broadcast_competitor_broadcast_id_competing_country_id] FOREIGN KEY ([broadcast_id], [competing_country_id]) REFERENCES [placeholder].[broadcast_competitor] ([broadcast_id], [competing_country_id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE INDEX [IX_broadcast_competitor_broadcast_id_performing_spot]
    ON [placeholder].[broadcast_competitor]([broadcast_id], [performing_spot]);


GO
CREATE UNIQUE INDEX [IX_broadcast_competitor_points_award_broadcast_id_competing_country_id_voting_country_id_voting_method]
    ON [placeholder].[broadcast_competitor_points_award]([broadcast_id], [competing_country_id], [voting_country_id], [voting_method]);


GO
CREATE INDEX [IX_broadcast_competitor_points_award_competing_country_id]
    ON [placeholder].[broadcast_competitor_points_award]([competing_country_id]);


GO
CREATE INDEX [IX_broadcast_competitor_points_award_voting_country_id]
    ON [placeholder].[broadcast_competitor_points_award]([voting_country_id]);


GO
INSERT  INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES                              (N'20260205063536_Create_placeholder_broadcast_aggregate_tables', N'10.0.2');
