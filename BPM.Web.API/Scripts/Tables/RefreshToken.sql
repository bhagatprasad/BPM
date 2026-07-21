CREATE TABLE RefreshToken
(
    id UUID NOT NULL DEFAULT gen_random_uuid(),
    userid UUID NOT NULL,
    refreshtokenvalue TEXT NOT NULL,
    jwttokenid VARCHAR(500),
    createdon TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    expireson TIMESTAMP NOT NULL,
    revokedon TIMESTAMP,
    replacedbytoken TEXT,
    isrevoked BOOLEAN NOT NULL DEFAULT FALSE,
    ipaddress VARCHAR(50),
    useragent TEXT,
    createdby UUID,
    modifiedby UUID,
    modifiedon TIMESTAMP,

    CONSTRAINT refresh_tokens_pkey PRIMARY KEY (id),

    CONSTRAINT fk_refresh_tokens_users
        FOREIGN KEY (userid)
        REFERENCES users(id)
        ON DELETE CASCADE
);
