CREATE TABLE user_password_history
(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    userid UUID NOT NULL,

    passwordhash TEXT NOT NULL,

    passwordsalt TEXT NOT NULL,

    createdon TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_user_password_history_users
        FOREIGN KEY(userid)
        REFERENCES users(id)
        ON DELETE CASCADE
);
