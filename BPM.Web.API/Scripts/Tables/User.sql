CREATE TABLE public.users
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    firstname character varying(100) COLLATE pg_catalog."default" NOT NULL,
    lastname character varying(100) COLLATE pg_catalog."default" NOT NULL,
    email character varying(150) COLLATE pg_catalog."default" NOT NULL,
    phone character varying(20) COLLATE pg_catalog."default",
    roleid uuid NOT NULL,
    dealerid uuid,
    passwordhash text COLLATE pg_catalog."default" NOT NULL,
    passwordsalt text COLLATE pg_catalog."default" NOT NULL,
    lastlogin timestamp without time zone,
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT users_pkey PRIMARY KEY (id),
    CONSTRAINT users_email_key UNIQUE (email),
    CONSTRAINT fk_users_dealers FOREIGN KEY (dealerid)
        REFERENCES public.dealers (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_users_roles FOREIGN KEY (roleid)
        REFERENCES public.roles (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)