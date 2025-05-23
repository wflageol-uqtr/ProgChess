import { createContext, useContext, useEffect, useMemo, useState } from "react";
import { hasCookie } from "../utils/cookie";
import { api } from "../utils/api";

const CookieContext = createContext();

const CookieProvider = ({ children }: any) => {
  const [cookie, setCookie] = useState<boolean>(hasCookie("studentCookie"));
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    console.log("ici");

    if (hasCookie("studentCode")) {
      console.log("laaa");
      verifyCookie();
    } else {
      console.log("loading stop");

      setIsLoading(false);
    }
  }, []);

  const verifyCookie = async () => {
    try {
      await api.get("/api/auth/verify-cookie");
    } catch (error) {
      console.log(error);
      setCookie(false);
    } finally {
      setIsLoading(false);
    }
  };

  const contextValue = useMemo(
    () => ({
      cookie,
      setCookie,
      isLoading,
    }),
    [cookie, isLoading]
  );

  return (
    <CookieContext.Provider value={contextValue}>
      {children}
    </CookieContext.Provider>
  );
};

export const useCookie = () => {
  return useContext(CookieContext);
};

export default CookieProvider;
