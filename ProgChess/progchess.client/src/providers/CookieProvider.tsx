import { createContext, useContext, useEffect, useMemo, useState } from "react";
import { hasCookie } from "../utils/cookie";
import { useParams } from "react-router";
import axios from "axios";

interface CookieContextType {
  cookie: boolean;
  isLoading: boolean;
}

const CookieContext = createContext<CookieContextType>();

const CookieProvider = ({ children }: any) => {
  const { id } = useParams();
  const [cookie, setCookie] = useState<boolean>(hasCookie("studentCookie"));
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    if (hasCookie("studentCookie")) {
      verifyCookie();
    } else {
      setIsLoading(false);
    }
  }, []);

  const verifyCookie = async () => {
    try {
      await axios.get("http://localhost:5290/api/auth/verify-cookie", {
        params: {
          exerciseId: parseInt(id!),
        },
        withCredentials: true,
      });
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
