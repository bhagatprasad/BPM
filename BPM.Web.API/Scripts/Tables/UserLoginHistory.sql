CREATE TABLE userloginhistory
(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    user_id UUID NULL,

    username VARCHAR(100) NOT NULL,

    login_time TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),

    logout_time TIMESTAMPTZ NULL,

    is_login_successful BOOLEAN NOT NULL,

    failure_reason VARCHAR(250),

    ip_address VARCHAR(50),

    user_agent TEXT,

    browser_name VARCHAR(100),

    operating_system VARCHAR(100),

    device_name VARCHAR(200),

    location VARCHAR(200),

    jwt_token_id VARCHAR(150),

    session_id UUID,

    created_on TIMESTAMPTZ NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
);