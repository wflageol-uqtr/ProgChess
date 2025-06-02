import { createContext, useContext, useEffect, useMemo, useState } from "react";
import { hasCookie } from "../utils/cookie";
import api from "../utils/api";

interface CookieContextType {
  cookie: boolean;
  isLoading: boolean;
}

const CookieContext = createContext<CookieContextType>();

const CookieProvider = ({ children }: any) => {
  const [cookie, setCookie] = useState<boolean>(hasCookie("studentCookie"));
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    if (hasCookie("studentCode")) {
      verifyCookie();
    } else {
      setIsLoading(false);
    }
  }, []);

  const verifyCookie = async () => {
    try {
      await api.get("/api/auth/verify-cookie");
    } catch (error) {
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
