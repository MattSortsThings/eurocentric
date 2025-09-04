/*
-- =============================================
-- Procedure:   Seeding v0 1: Add 50 countries
-- Author:      Matt Tantony
-- Create date: 2025-09-04
--
-- Description:
--  Populates tables in the 'v0' schema with data for 50 countries.
-- =============================================
*/

BEGIN TRANSACTION

INSERT INTO v0.country (id, country_code, country_name, participating_contest_ids)
VALUES (N'01979615-1E4C-7BA1-868B-018CE12E1C0C', N'GB', N'United Kingdom', N'[]'),
       (N'01979614-F0F2-75EF-9571-0651FA138830', N'CZ', N'Czechia', N'[]'),
       (N'01979615-6882-7BAD-AD78-0EE9B7EF6FE3', N'MD', N'Moldova', N'[]'),
       (N'01979615-57B1-7FB7-8B08-18CB9DF96425', N'LV', N'Latvia', N'[]'),
       (N'01979615-B878-7BE8-A492-1D63ECD4FB44', N'TK', N'Türkiye', N'[]'),
       (N'01979615-01F7-7F25-9C64-271204D67F4E', N'EE', N'Estonia', N'[]'),
       (N'01979615-34FB-74C6-92FD-2A6DF101475E', N'IE', N'Ireland', N'[]'),
       (N'01979615-A771-7494-BA80-2CB6CFC8E75F', N'SI', N'Slovenia', N'[]'),
       (N'01979614-DFEA-719B-8E8B-2E8A15925E89', N'BY', N'Belarus', N'[]'),
       (N'01979615-AD16-7D0A-8DCC-31E1E592188A', N'SK', N'Slovakia', N'[]'),
       (N'01979614-B7F0-7DFB-BC52-363409167C63', N'AM', N'Armenia', N'[]'),
       (N'01979614-FC50-719F-BF26-488F30D9B892', N'DK', N'Denmark', N'[]'),
       (N'01979615-C3CC-76AC-BCC6-491975D0329A', N'XX', N'Rest of the World', N'[]'),
       (N'01979614-C8FB-7760-97B9-4DFEE3CC1BA2', N'AZ', N'Azerbaijan', N'[]'),
       (N'01979615-130B-744D-8624-4EB9B9419CE8', N'FR', N'France', N'[]'),
       (N'01979615-9662-7B75-962D-5435F35CF221', N'RS', N'Serbia', N'[]'),
       (N'01979614-BD9E-796C-BEDA-545301FDA84D', N'AT', N'Austria', N'[]'),
       (N'01979614-CEB5-72A6-941A-56DACF355264', N'BA', N'Bosnia & Herzegovina', N'[]'),
       (N'01979615-18B2-71F9-8DAA-57E3F523E374', N'GE', N'Georgia', N'[]'),
       (N'01979615-4048-7F62-8844-5CB587DAAA48', N'IS', N'Iceland', N'[]'),
       (N'01979614-E5A6-7FF2-BB06-62289D36AD5D', N'CH', N'Switzerland', N'[]'),
       (N'01979615-9098-7280-B450-64C6687EF850', N'RO', N'Romania', N'[]'),
       (N'01979615-73EB-7C50-A078-669847B5D59B', N'MT', N'Malta', N'[]'),
       (N'01979615-B2C4-7187-BE8D-67637792D762', N'SM', N'San Marino', N'[]'),
       (N'01979614-AAC8-705F-AB85-6F2EED244072', N'AD', N'Andorra', N'[]'),
       (N'01979615-6E2C-7926-8FAA-6F61EE35E7FC', N'ME', N'Montenegro', N'[]'),
       (N'01979614-C353-74DA-AF0B-6FD244E38672', N'AU', N'Australia', N'[]'),
       (N'01979615-8535-7637-BF10-78B971054220', N'PL', N'Poland', N'[]'),
       (N'01979615-62EE-7939-9DCE-791201394C25', N'MK', N'North Macedonia', N'[]'),
       (N'01979615-9C22-7164-A6C9-7FCA5FFC93A5', N'RU', N'Russia', N'[]'),
       (N'01979615-8AF5-78B7-9C01-80FC94530350', N'PT', N'Portugal', N'[]'),
       (N'01979615-BE37-7455-9A5E-8CAA9FE8081A', N'UA', N'Ukraine', N'[]'),
       (N'01979615-5D53-756A-A821-915B502E2C14', N'MC', N'Monaco', N'[]'),
       (N'01979615-3AA9-7C0D-9C12-9818ED910470', N'IL', N'Israel', N'[]'),
       (N'01979615-4C55-719F-8350-AA57AD3F8EE5', N'LT', N'Lithuania', N'[]'),
       (N'01979615-23FB-7539-8CB0-B4118D96C9BF', N'GR', N'Greece', N'[]'),
       (N'01979615-0D59-7ECD-907D-B642BF60F0E0', N'FI', N'Finland', N'[]'),
       (N'01979615-A1C9-71CB-90BF-B7A6F7AE13B2', N'SE', N'Sweden', N'[]'),
       (N'01979615-4604-7D91-A989-B7AA4AF73714', N'IT', N'Italy', N'[]'),
       (N'01979614-F69B-7E3D-975D-BE025FB0EC4B', N'DE', N'Germany', N'[]'),
       (N'01979615-79A6-7B7B-AD28-D29D2004DEA8', N'NL', N'Netherlands', N'[]'),
       (N'01979614-B245-791F-BE0F-D9C8C03BDF68', N'AL', N'Albania', N'[]'),
       (N'01979615-5209-741A-81FC-DD1B6AEA1961', N'LU', N'Luxembourg', N'[]'),
       (N'01979614-EB53-7AC0-850B-DEB307FC7B63', N'CY', N'Cyprus', N'[]'),
       (N'01979615-7F6F-7CEE-94D4-E3613CE86757', N'NO', N'Norway', N'[]'),
       (N'01979615-29B5-7909-BF04-EE006DB409E1', N'HR', N'Croatia', N'[]'),
       (N'01979615-2F51-7475-83A6-F17ED5B1D769', N'HU', N'Hungary', N'[]'),
       (N'01979614-DA3B-7DD1-944B-F8F6AA58FB15', N'BG', N'Bulgaria', N'[]'),
       (N'01979615-07B6-7590-B005-FBAA9A2509E1', N'ES', N'Spain', N'[]'),
       (N'01979614-D481-763E-8BBF-FE7151130728', N'BE', N'Belgium', N'[]');

COMMIT
