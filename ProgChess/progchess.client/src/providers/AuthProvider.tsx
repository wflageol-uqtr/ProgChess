import {
  createContext,
  useContext,
  useEffect,
  useLayoutEffect,
  useMemo,
  useState,
} from "react";
import axios from "axios";

interface AuthContextType {
  token: string | null;
}

const AuthContext = createContext<AuthContextType>();

const AuthProvider = ({ children }: any) => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("accessToken")
  );

  useEffect(() => {
    const verifyToken = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5290/api/auth/verify-token",
          { withCredentials: true }
        );
        console.log(localStorage.getItem("accessToken"));
        setToken(localStorage.getItem("accessToken"));
      } catch (error) {
        setToken(null);
      }
    };

    verifyToken();
  }, []);

  useLayoutEffect(() => {
    if (token) {
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    } else {
      delete axios.defaults.headers.common["Authorization"];
    }
  });

  useLayoutEffect(() => {
    const refreshInterceptor = axios.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;

        if (
          error.response.data.status == 401 &&
          error.response.data.message === "Unauthorized"
        ) {
          try {
            const response = await axios.get(
              "http://localhost:5290/api/auth/refresh-token"
            );
            setToken(response.data.accessToken);
            axios.defaults.headers.common["Authorization"] =
              "Bearer " + response.data.accessToken;
            originalRequest._retry = true;

            return axios(originalRequest);
          } catch (error) {
            setToken(null);
          }
        }
        return Promise.reject(error);
      }
    );

    return () => {
      axios.interceptors.response.eject(refreshInterceptor);
    };
  });

  const contextValue = useMemo(
    () => ({
      token,
      setToken,
    }),
    [token]
  );

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export default AuthProvider;
