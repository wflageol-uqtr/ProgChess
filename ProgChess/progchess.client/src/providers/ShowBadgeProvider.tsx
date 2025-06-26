import { createContext, useContext, useState } from "react";

const BadgeContext = createContext();

// En parler avec William
export const BadgeProvider = ({ children }: any) => {
  const [badgeTabs, setBadgeTabs] = useState({
    result: false,
    tests: false,
    errors: false,
  });

  return (
    <BadgeContext.Provider value={{ badgeTabs, setBadgeTabs }}>
      {children}
    </BadgeContext.Provider>
  );
};

export const useBadge = () => {
  const context = useContext(BadgeContext);
  if (!context) throw new Error("useBadge must be used inside <BadgeProvider>");
  return context;
};
